using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Services.AboutPageContents;

public sealed class AboutPageContentService : IAboutPageContentService
{
    private readonly IUnitOfWork _unitOfWork;

    public AboutPageContentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<AboutPageContentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _unitOfWork.Repository<AboutPageContent>().GetAllAsync(cancellationToken);
        return records.OrderBy(item => item.Id).Select(Map).ToList();
    }

    public async Task<AboutPageContentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var record = await _unitOfWork.Repository<AboutPageContent>().GetByIdAsync(id, cancellationToken);
        return record is null ? null : Map(record);
    }

    public async Task<AboutPageContentDto> CreateAsync(UpsertAboutPageContentRequest request, CancellationToken cancellationToken = default)
    {
        var record = Map(request);

        await _unitOfWork.Repository<AboutPageContent>().AddAsync(record, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(record);
    }

    public async Task<bool> UpdateAsync(int id, UpsertAboutPageContentRequest request, CancellationToken cancellationToken = default)
    {
        var record = await _unitOfWork.Repository<AboutPageContent>().GetByIdAsync(id, cancellationToken);
        if (record is null)
        {
            return false;
        }

        Apply(record, request);

        _unitOfWork.Repository<AboutPageContent>().Update(record);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var record = await _unitOfWork.Repository<AboutPageContent>().GetByIdAsync(id, cancellationToken);
        if (record is null)
        {
            return false;
        }

        _unitOfWork.Repository<AboutPageContent>().Remove(record);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static AboutPageContent Map(UpsertAboutPageContentRequest request)
    {
        var record = new AboutPageContent();
        Apply(record, request);
        return record;
    }

    private static void Apply(AboutPageContent record, UpsertAboutPageContentRequest request)
    {
        record.MainTitleAr = request.MainTitleAr;
        record.MainTitleEn = request.MainTitleEn;
        record.SubTitleAr = request.SubTitleAr;
        record.SubTitleEn = request.SubTitleEn;
        record.IntroTextAr = request.IntroTextAr;
        record.IntroTextEn = request.IntroTextEn;
        record.WhyChooseTitleAr = request.WhyChooseTitleAr;
        record.WhyChooseTitleEn = request.WhyChooseTitleEn;
        record.WhyChooseDescriptionAr = request.WhyChooseDescriptionAr;
        record.WhyChooseDescriptionEn = request.WhyChooseDescriptionEn;
        record.WhatWeOfferTitleAr = request.WhatWeOfferTitleAr;
        record.WhatWeOfferTitleEn = request.WhatWeOfferTitleEn;
        record.WhatWeOfferDescriptionAr = request.WhatWeOfferDescriptionAr;
        record.WhatWeOfferDescriptionEn = request.WhatWeOfferDescriptionEn;
        record.MissionTitleAr = request.MissionTitleAr;
        record.MissionTitleEn = request.MissionTitleEn;
        record.MissionDescriptionAr = request.MissionDescriptionAr;
        record.MissionDescriptionEn = request.MissionDescriptionEn;
        record.WhereWeWorkTitleAr = request.WhereWeWorkTitleAr;
        record.WhereWeWorkTitleEn = request.WhereWeWorkTitleEn;
        record.WhereWeWorkDescriptionAr = request.WhereWeWorkDescriptionAr;
        record.WhereWeWorkDescriptionEn = request.WhereWeWorkDescriptionEn;
        record.Image = request.Image;
    }

    private static AboutPageContentDto Map(AboutPageContent record)
    {
        return new AboutPageContentDto(
            record.Id,
            record.MainTitleAr,
            record.MainTitleEn,
            record.SubTitleAr,
            record.SubTitleEn,
            record.IntroTextAr,
            record.IntroTextEn,
            record.WhyChooseTitleAr,
            record.WhyChooseTitleEn,
            record.WhyChooseDescriptionAr,
            record.WhyChooseDescriptionEn,
            record.WhatWeOfferTitleAr,
            record.WhatWeOfferTitleEn,
            record.WhatWeOfferDescriptionAr,
            record.WhatWeOfferDescriptionEn,
            record.MissionTitleAr,
            record.MissionTitleEn,
            record.MissionDescriptionAr,
            record.MissionDescriptionEn,
            record.WhereWeWorkTitleAr,
            record.WhereWeWorkTitleEn,
            record.WhereWeWorkDescriptionAr,
            record.WhereWeWorkDescriptionEn,
            record.Image);
    }
}
