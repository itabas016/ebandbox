--CREATE INDEX ix_source ON NewsSource (NameLowCase,[Status]);

--CREATE INDEX ix_category ON NewsCategory ([Status]);

--CREATE INDEX ix_subcategory ON NewsSubCategory (DisplayName,SourceId,CategoryId,[Status]);

ALTER TABLE newssource ADD CONSTRAINT uc_source_pkg_name UNIQUE (PackageName);

ALTER TABLE toutiaocontent ADD CONSTRAINT uc_content_newsid UNIQUE (NewsId);

CREATE INDEX ix_toutiaocontent_categoryid ON toutiaocontent (CategoryId);

CREATE INDEX ix_toutiaocontent_subcategoryid ON toutiaocontent (SubCategoryId);

CREATE INDEX ix_toutiaocontent_status ON toutiaocontent (Status);

CREATE INDEX ix_toutiaocontent_publish_time ON toutiaocontent (PublishTime);

CREATE INDEX ix_toutiaocontent_create_date ON toutiaocontent (CreateDateTime);

CREATE INDEX ix_imageinfo_newsid ON newsimageinfo (NewsId);

CREATE INDEX ix_imageinfo_status ON newsimageinfo (Status);