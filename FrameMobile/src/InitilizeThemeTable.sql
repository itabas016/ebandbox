--WallPaper
ALTER TABLE wallpaper ADD CONSTRAINT uc_wallpaper_no UNIQUE (WallPaperNo);

CREATE INDEX ix_wallpaper_publishtime ON wallpaper (PublishTime);

CREATE INDEX ix_wallpaper_downloadnumber ON wallpaper (DownloadNumber);

CREATE INDEX ix_wallpaper_status ON wallpaper (Status);

--WallPaperSubCategory
CREATE INDEX ix_wallpapersubcategory_categoryid ON wallpapersubcategory (CategoryId);

--Mobile
CREATE INDEX ix_mobileproperty_brandid ON mobileproperty (BrandId);

CREATE INDEX ix_mobileproperty_hardwareid ON mobileproperty (HardwareId);

CREATE INDEX ix_mobileproperty_resolutionid ON mobileproperty (ResolutionId);

CREATE INDEX ix_mobileproperty_status ON mobileproperty (Status);

--WallPaperRelateCategory
CREATE INDEX ix_wallpaperrelatecategory_wallpaperid ON wallpaperrelatecategory (WallPaperId);

CREATE INDEX ix_wallpaperrelatecategory_categoryid ON wallpaperrelatecategory (CategoryId);

CREATE INDEX ix_wallpaperrelatecategory_status ON wallpaperrelatecategory (Status);

--WallPaperRelateSubCategory
CREATE INDEX ix_wallpaperrelatesubcategory_wallpaperid ON wallpaperrelatesubcategory (WallPaperId);

CREATE INDEX ix_wallpaperrelatesubcategory_subcategoryid ON wallpaperrelatesubcategory (SubCategoryId);

CREATE INDEX ix_wallpaperrelatesubcategory_status ON wallpaperrelatesubcategory (Status);

--WallPaperRelateTopic
CREATE INDEX ix_wallpaperrelatetopic_wallpaperid ON wallpaperrelatetopic (WallPaperId);

CREATE INDEX ix_wallpaperrelatetopic_subcategoryid ON wallpaperrelatetopic (TopicId);

CREATE INDEX ix_wallpaperrelatetopic_status ON wallpaperrelatetopic (Status);

--WallPaperRelateMobileProperty
CREATE INDEX ix_wallpaperrelatemobileproperty_wallpaperid ON wallpaperrelatemobileproperty (WallPaperId);

CREATE INDEX ix_wallpaperrelatemobileproperty_subcategoryid ON wallpaperrelatemobileproperty (MobilePropertyId);

CREATE INDEX ix_wallpaperrelatemobileproperty_status ON wallpaperrelatemobileproperty (Status);

