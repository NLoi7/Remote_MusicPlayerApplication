use ClientMusic
create table ListSong(
	ID int primary key,
	NameSong nvarchar(100),
	AuthorInfor nvarchar(50) 
)
go
insert into ListSong values(1001,N'Muộn Rồi Mà Sao Còn',N'Sơn Tùng - MTP')

insert into ListSong values(1002,N'Hơn cả yêu',N'Đức Phúc')

insert into ListSong values(1003,N'Nến và Hoa',N'Rhymastic')
insert into ListSong values(1004,N'Thức Giấc',N'Da Lab')
insert into ListSong values(1005,N'Chìm Sâu',N'RPT MCK')

go
delete from ListSong
create table InforAuthor( 
	ID int foreign key references ListSong(ID),
	AuthorName nvarchar(50),
	NameSongOfSinger nvarchar(100),
	publishingYear datetime,
	SongType nvarchar(50)
	)
insert into InforAuthor values (1001,N'Sơn Tùng - MTP',N'Lạc Trôi',2020-12-31,N'Future bass, R&B/Soul')
insert into InforAuthor values (1001,N'Sơn Tùng - MTP',N'Nắng Ấm Xa Dần',2013-12-1,N'Pop')
insert into InforAuthor values (1001,N'Sơn Tùng - MTP',N'Muộn Rồi Mà Sao Còn',2021-4-29,N'R&B Pop')
insert into InforAuthor values (1001,N'Sơn Tùng - MTP',N'Có Chắc Yêu Là Đây',2020-7-5,N'R&B Pop - Rap')
insert into InforAuthor values (1001,N'Sơn Tùng - MTP',N'Chạy Ngay Đi',2018-5-12,N'R&B Pop - TRap')
insert into InforAuthor values (1002,N'Erik',N'Có tất cả nhưng thiếu anh',2019-7-28,N'Pop')


select * from InforAuthor as ifo  inner join ListSong 
on ifo.ID=ListSong.ID 