--CREATE INDEX ix_source ON NewsSource (NameLowCase,[Status]);

--CREATE INDEX ix_category ON NewsCategory ([Status]);

--CREATE INDEX ix_subcategory ON NewsSubCategory (DisplayName,SourceId,CategoryId,[Status]);

ALTER TABLE NewsSource ADD CONSTRAINT uc_source_pkg_name UNIQUE (PackageName);

ALTER TABLE TouTiaoContent ADD CONSTRAINT uc_content_newsid UNIQUE (NewsId);

CREATE INDEX ix_toutiaocontent_categoryid ON TouTiaoContent (CategoryId);

CREATE INDEX ix_toutiaocontent_status ON TouTiaoContent (Status);

CREATE INDEX ix_toutiaocontent_publish_time ON TouTiaoContent (PublishTime);

CREATE INDEX ix_toutiaocontent_create_date ON TouTiaoContent (CreateDateTime);

CREATE INDEX ix_imageinfo_newsid ON NewsImageInfo (NewsId);

CREATE INDEX ix_imageinfo_status ON NewsImageInfo (Status);