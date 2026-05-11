namespace SidraHub.Application.Services.AboutPageContents;

public sealed class UpsertAboutPageContentRequest
{
    public string MainTitleAr { get; set; } = string.Empty;
    public string MainTitleEn { get; set; } = string.Empty;
    public string SubTitleAr { get; set; } = string.Empty;
    public string SubTitleEn { get; set; } = string.Empty;
    public string IntroTextAr { get; set; } = string.Empty;
    public string IntroTextEn { get; set; } = string.Empty;
    public string WhyChooseTitleAr { get; set; } = string.Empty;
    public string WhyChooseTitleEn { get; set; } = string.Empty;
    public string WhyChooseDescriptionAr { get; set; } = string.Empty;
    public string WhyChooseDescriptionEn { get; set; } = string.Empty;
    public string WhatWeOfferTitleAr { get; set; } = string.Empty;
    public string WhatWeOfferTitleEn { get; set; } = string.Empty;
    public string WhatWeOfferDescriptionAr { get; set; } = string.Empty;
    public string WhatWeOfferDescriptionEn { get; set; } = string.Empty;
    public string MissionTitleAr { get; set; } = string.Empty;
    public string MissionTitleEn { get; set; } = string.Empty;
    public string MissionDescriptionAr { get; set; } = string.Empty;
    public string MissionDescriptionEn { get; set; } = string.Empty;
    public string WhereWeWorkTitleAr { get; set; } = string.Empty;
    public string WhereWeWorkTitleEn { get; set; } = string.Empty;
    public string WhereWeWorkDescriptionAr { get; set; } = string.Empty;
    public string WhereWeWorkDescriptionEn { get; set; } = string.Empty;
    public string? Image { get; set; }
}
