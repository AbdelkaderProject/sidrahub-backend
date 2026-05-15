using Microsoft.EntityFrameworkCore;
using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Services.ContactMessages;

public sealed class ContactMessageService : IContactMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly INotificationService _notificationService;

    public ContactMessageService(IUnitOfWork unitOfWork, IApplicationDbContext context, IEmailService emailService, INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        _emailService = emailService;
        _notificationService = notificationService;
    }

    public async Task<IReadOnlyList<ContactMessageDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var messages = await _context.ContactMessages
            .OrderByDescending(m => m.CreatedDateTime)
            .ToListAsync(cancellationToken);
        return messages.Select(Map).ToList();
    }

    public async Task<ContactMessageDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var message = await _unitOfWork.Repository<ContactMessage>().GetByIdAsync(id, cancellationToken);
        return message is null ? null : Map(message);
    }

    public async Task<ContactMessageDto> CreateAsync(CreateContactMessageRequest request, CancellationToken cancellationToken = default)
    {
        var message = new ContactMessage
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Subject = request.Subject,
            Message = request.Message,
            IsRead = false
        };

        await _unitOfWork.Repository<ContactMessage>().AddAsync(message, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Notify admins
        await _notificationService.NotifyAdminsAsync(
            "📩 رسالة تواصل جديدة",
            $"رسالة جديدة من {message.Name} - الموضوع: {message.Subject}",
            cancellationToken);

        // Send confirmation email to the user
        await SendUserConfirmationEmailAsync(message, cancellationToken);

        // Send notification email to admin
        await SendAdminNotificationEmailAsync(message, cancellationToken);

        return Map(message);
    }

    public async Task<bool> MarkAsReadAsync(int id, CancellationToken cancellationToken = default)
    {
        var message = await _unitOfWork.Repository<ContactMessage>().GetByIdAsync(id, cancellationToken);
        if (message is null) return false;

        message.IsRead = true;
        _unitOfWork.Repository<ContactMessage>().Update(message);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var message = await _unitOfWork.Repository<ContactMessage>().GetByIdAsync(id, cancellationToken);
        if (message is null) return false;

        _unitOfWork.Repository<ContactMessage>().Remove(message);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task SendUserConfirmationEmailAsync(ContactMessage message, CancellationToken cancellationToken)
    {
        try
        {
            var subject = "تم استلام رسالتك - Alternative Option";
            var body = $"""
                <div dir="rtl" style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; background-color: #f9f9f9;">
                    <div style="background: linear-gradient(135deg, #0b2849 0%, #1a4a7a 100%); padding: 30px; border-radius: 12px 12px 0 0; text-align: center;">
                        <h1 style="color: #ffffff; margin: 0; font-size: 24px;">Alternative Option</h1>
                        <p style="color: #c3954c; margin: 8px 0 0 0;">للاستشارات وحلول الأعمال</p>
                    </div>
                    <div style="background: #ffffff; padding: 30px; border-radius: 0 0 12px 12px; border: 1px solid #e2e8f0;">
                        <h2 style="color: #0b2849; margin-top: 0;">شكراً لتواصلك معنا، {message.Name}!</h2>
                        <p style="color: #475569; line-height: 1.6;">لقد استلمنا رسالتك بنجاح وسنقوم بالرد عليك في أقرب وقت ممكن.</p>
                        <div style="background: #f8fafc; border-right: 4px solid #c3954c; padding: 16px; margin: 20px 0; border-radius: 4px;">
                            <p style="margin: 0; color: #334155;"><strong>الموضوع:</strong> {message.Subject}</p>
                        </div>
                        <p style="color: #475569; line-height: 1.6;">إذا كان لديك أي استفسار عاجل، يمكنك التواصل معنا مباشرة على:</p>
                        <p style="color: #0b2849;"><strong>📧 Info@Alternative-Option.com</strong></p>
                        <p style="color: #0b2849;"><strong>📞 00201120023100</strong></p>
                        <hr style="border: none; border-top: 1px solid #e2e8f0; margin: 24px 0;" />
                        <p style="color: #94a3b8; font-size: 12px; text-align: center; margin: 0;">
                            Alternative Option للاستشارات وحلول الأعمال - مصر، القاهرة
                        </p>
                    </div>
                </div>
                """;

            await _emailService.SendAsync(message.Email, message.Name, subject, body, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Email] Failed to send user confirmation: {ex.Message}");
        }
    }

    private async Task SendAdminNotificationEmailAsync(ContactMessage message, CancellationToken cancellationToken)
    {
        try
        {
            var subject = $"رسالة تواصل جديدة من {message.Name} - {message.Subject}";
            var body = $"""
                <div style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;">
                    <div style="background: linear-gradient(135deg, #0b2849 0%, #1a4a7a 100%); padding: 20px; border-radius: 8px 8px 0 0;">
                        <h2 style="color: #ffffff; margin: 0;">رسالة تواصل جديدة</h2>
                    </div>
                    <div style="background: #ffffff; padding: 24px; border: 1px solid #e2e8f0; border-radius: 0 0 8px 8px;">
                        <table style="width: 100%; border-collapse: collapse;">
                            <tr>
                                <td style="padding: 10px; background: #f8fafc; font-weight: bold; width: 30%; border: 1px solid #e2e8f0;">الاسم</td>
                                <td style="padding: 10px; border: 1px solid #e2e8f0;">{message.Name}</td>
                            </tr>
                            <tr>
                                <td style="padding: 10px; background: #f8fafc; font-weight: bold; border: 1px solid #e2e8f0;">البريد الإلكتروني</td>
                                <td style="padding: 10px; border: 1px solid #e2e8f0;">{message.Email}</td>
                            </tr>
                            <tr>
                                <td style="padding: 10px; background: #f8fafc; font-weight: bold; border: 1px solid #e2e8f0;">رقم الجوال</td>
                                <td style="padding: 10px; border: 1px solid #e2e8f0;">{message.Phone}</td>
                            </tr>
                            <tr>
                                <td style="padding: 10px; background: #f8fafc; font-weight: bold; border: 1px solid #e2e8f0;">الموضوع</td>
                                <td style="padding: 10px; border: 1px solid #e2e8f0;">{message.Subject}</td>
                            </tr>
                            <tr>
                                <td style="padding: 10px; background: #f8fafc; font-weight: bold; border: 1px solid #e2e8f0;">الرسالة</td>
                                <td style="padding: 10px; border: 1px solid #e2e8f0;">{message.Message}</td>
                            </tr>
                            <tr>
                                <td style="padding: 10px; background: #f8fafc; font-weight: bold; border: 1px solid #e2e8f0;">التاريخ</td>
                                <td style="padding: 10px; border: 1px solid #e2e8f0;">{message.CreatedDateTime:yyyy-MM-dd HH:mm}</td>
                            </tr>
                        </table>
                    </div>
                </div>
                """;

            await _emailService.SendAsync("Info@Alternative-Option.com", "Admin", subject, body, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Email] Failed to send admin notification: {ex.Message}");
        }
    }

    private static ContactMessageDto Map(ContactMessage m) => new(
        m.Id, m.Name, m.Email, m.Phone, m.Subject, m.Message, m.IsRead, m.CreatedDateTime);
}
