--CREATE INDEX ix_source ON NewsSource (NameLowCase,[Status]);

--CREATE INDEX ix_category ON NewsCategory ([Status]);

--CREATE INDEX ix_subcategory ON NewsSubCategory (Name,SourceId,CategoryId,[Status]);

ALTER TABLE newssource ADD CONSTRAINT uc_source_pkg_name UNIQUE (PackageName);

ALTER TABLE newscontent ADD CONSTRAINT uc_content_newsid UNIQUE (NewsId);

CREATE INDEX ix_newscontent_extraappid ON newscontent (ExtraAppId);

CREATE INDEX ix_newscontent_categoryid ON newscontent (CategoryId);

CREATE INDEX ix_newscontent_subcategoryid ON newscontent (SubCategoryId);

CREATE INDEX ix_newscontent_status ON newscontent (Status);

CREATE INDEX ix_newscontent_publish_time ON newscontent (PublishTime);

CREATE INDEX ix_newscontent_create_date ON newscontent (CreateDateTime);

CREATE INDEX ix_imageinfo_newsid ON newsimageinfo (NewsId);

CREATE INDEX ix_imageinfo_status ON newsimageinfo (Status);