--Add NewsCategory
INSERT INTO newscategory (Id,Name,Status,CreateDateTime) VALUES(1,"热点",1,NOW());
INSERT INTO newscategory (Id,Name,Status,CreateDateTime) VALUES(2,"科技",1,NOW());
INSERT INTO newscategory (Id,Name,Status,CreateDateTime) VALUES(3,"娱乐",1,NOW());
INSERT INTO newscategory (Id,Name,Status,CreateDateTime) VALUES(4,"体育",1,NOW());
INSERT INTO newscategory (Id,Name,Status,CreateDateTime) VALUES(5,"财经",1,NOW());
INSERT INTO newscategory (Id,Name,Status,CreateDateTime) VALUES(6,"生活",1,NOW());

--Add NewsExtraApp
INSERT INTO newsextraapp (Id,Name,NameLowCase,PackageName,IsBrower,DownloadURL,Status,CreateDateTime)
VALUES(1,"今日头条","toutiao","com.ss.android.article.news",0,"http://apk.oo523.com/appstores/apkdownload?imsi=0000&os=android&pkgname=com.ss.android.article.news",1,NOW());
INSERT INTO newsextraapp (Id,Name,NameLowCase,PackageName,IsBrower,DownloadURL,Status,CreateDateTime)
VALUES(2,"QQ浏览器","tentcent","com.tencent.mtt",1,"http://apk.oo523.com/appstores/apkdownload?imsi=0000&os=android&pkgname= com.tencent.mtt",1,NOW());
