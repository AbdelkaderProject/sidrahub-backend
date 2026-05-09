using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Services.Customers;

public sealed class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _unitOfWork.Repository<Customer>().GetAllAsync(cancellationToken);
        return customers.OrderBy(customer => customer.Id).Select(Map).ToList();
    }

    public async Task<CustomerDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(id, cancellationToken);
        return customer is null ? null : Map(customer);
    }

    public async Task<CustomerDto> CreateAsync(UpsertCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var customer = new Customer
        {
            NameAr = request.NameAr,
            NameEn = request.NameEn,
            Logo = request.Logo
        };

        await _unitOfWork.Repository<Customer>().AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(customer);
    }

    public async Task<bool> UpdateAsync(int id, UpsertCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(id, cancellationToken);
        if (customer is null)
        {
            return false;
        }

        customer.NameAr = request.NameAr;
        customer.NameEn = request.NameEn;
        customer.Logo = request.Logo;

        _unitOfWork.Repository<Customer>().Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(id, cancellationToken);
        if (customer is null)
        {
            return false;
        }

        _unitOfWork.Repository<Customer>().Remove(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static CustomerDto Map(Customer customer)
    {
        return new CustomerDto(
            customer.Id,
            customer.NameAr,
            customer.NameEn,
            customer.Logo);
    }
}
