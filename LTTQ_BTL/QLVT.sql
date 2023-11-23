CREATE DATABASE QLVuonThu
GO

DROP DATABASE QLVuonThu

USE [QLVuonThu]
GO


--************ Table NhanVien ************--
CREATE TABLE NhanVien
(
	MaNV NVARCHAR(10) NOT NULL, 
	TenNV NVARCHAR(50) NULL,
	NTNS DATETIME NULL,
	GTinh NVARCHAR(10) NULL,
	DiaChi NVARCHAR(50) NULL,
	SDT NVARCHAR(50) NULL,
	TenDN NVARCHAR(20) NULL,
	MatKhau NVARCHAR(20) NULL,
	PRIMARY KEY (MaNV)
) 

--************ Table Loai ************--
CREATE TABLE Loai
(
	MaLoai NVARCHAR(10) NOT NULL,
	TenLoai NVARCHAR(50) NULL,
	GhiChu NVARCHAR(50) NULL,
	PRIMARY KEY (MaLoai)
)

--************ Table NguonGoc ************--
--CREATE TABLE NguonGoc
--(
--	TenNguonGoc NVARCHAR(50) NOT NULL, 
--	TenKieuSinh NVARCHAR(50) NULL,
--	GhiChu NVARCHAR(50) NULL,
--	PRIMARY KEY (TenNguonGoc)
--)

--************ Table Thu ************--
CREATE TABLE Thu
(
	MaThu NVARCHAR(10) NOT NULL,
	TenNguonGoc NVARCHAR(50) NOT NULL,
	TenThu NVARCHAR(50) NULL,
	MaLoai NVARCHAR(10) NULL,
	SoLuong INT NULL,
	SachDo NVARCHAR(50) NULL,
	TenKhoaHoc NVARCHAR(50) NULL,
	TenTA NVARCHAR(50) NULL,
	TenTV NVARCHAR(50) NULL,
	KieuSinh NVARCHAR(50) NULL,
	NgayVao DATETIME NULL,
	DacDiem NVARCHAR(50) NULL,
	NgaySinh DATETIME NULL,
	Anh IMAGE NULL,
	TuoiTho INT NULL,
	PRIMARY KEY (MaThu),
	FOREIGN KEY (MaLoai) REFERENCES Loai(MaLoai)
) 

--************ Table TrangThai ************--
CREATE TABLE TrangThai
(
	MaTT NVARCHAR(10) NOT NULL,
	TenTT NVARCHAR(50) NULL, 
	GhiChu NVARCHAR(50) NULL,
	PRIMARY KEY (MaTT)
) 

--************ Table Khu ************--
CREATE TABLE Khu
(
	MaKhu NVARCHAR(10) NOT NULL,
	TenKhu NVARCHAR(50) NULL, 
	GhiChu NVARCHAR(50) NULL,
	PRIMARY KEY (MaKhu)
) 

--************ Table Chuong ************--
CREATE TABLE Chuong
(
	MaChuong NVARCHAR(10) NOT NULL,
	MaTT NVARCHAR(10) NOT NULL,
	MaNV NVARCHAR(10) NOT NULL,
	MaLoai NVARCHAR(10) NOT NULL,
	MaKhu NVARCHAR(10) NOT NULL,
	--MaThu NVARCHAR(10) NOT NULL,
	DienTich FLOAT NULL,
	ChieuCao FLOAT NULL,
	SLThu INT NULL,
	
	PRIMARY KEY (MaChuong),
	--FOREIGN KEY (MaChuong) REFERENCES ThuChuong(MaChuong),
	FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
	FOREIGN KEY (MaTT) REFERENCES TrangThai(MaTT),
	FOREIGN KEY (MaKhu) REFERENCES Khu(MaKhu),
	--FOREIGN KEY (MaThu) REFERENCES Thu(MaThu),
	FOREIGN KEY (MaLoai) REFERENCES Loai(MaLoai),
)
--************ Table ThuChuong ************--
CREATE TABLE ThuChuong
(
	MaChuong NVARCHAR(10) NOT NULL,
	MaThu NVARCHAR(10) NOT NULL,
	NgayVao DATETIME NULL,
	LyDoVao NVARCHAR(50) NULL,
	PRIMARY KEY(MaChuong, MaThu),
	FOREIGN KEY (MaChuong) REFERENCES Chuong(MaChuong),
	FOREIGN KEY (MaThu) REFERENCES Thu(MaThu),
)
--************ Table LyDo ************--
--CREATE TABLE LyDo
--(
--	MaLD NVARCHAR(10) NOT NULL,
--	TenLD NVARCHAR(50) NULL, 
--	GhiChu NVARCHAR(50) NULL,
--	PRIMARY KEY (MaLD)
--)

--************ Table KhacPhuc ************--
--CREATE TABLE KhacPhuc
--(
--	MaKP NVARCHAR(10) NOT NULL,
--	TenCachKP NVARCHAR(50) NULL,
--	GhiChu NVARCHAR(50) NULL,
--	PRIMARY KEY (MaKP)
--)

--************ Table SuKien ************--
CREATE TABLE SuKien
(
	MaSK NVARCHAR(10) NOT NULL,
	TenSK NVARCHAR(50) NULL,
	GhiChu NVARCHAR(50) NULL,
	PRIMARY KEY (MaSK)
)

--************ Table Thu_SuKien ************--
CREATE TABLE Thu_SuKien
(
	MaSK NVARCHAR(10) NOT NULL,
	MaThu NVARCHAR(10) NOT NULL,
	NgayBD DATETIME NULL,
	TenLD NVARCHAR(50) NOT NULL,
	CachKP NVARCHAR(50) NOT NULL,
	NgayKT DATETIME NULL,
	FOREIGN KEY (MaSK) REFERENCES SuKien(MaSK),
	FOREIGN KEY (MaThu) REFERENCES Thu(MaThu)
)

--************ Table ThucAn ************--
CREATE TABLE ThucAn
(
	MaTA NVARCHAR(10) NOT NULL,
	Ten NVARCHAR(50) NULL,
	CongDung NVARCHAR(50) NULL,
	MaDV NVARCHAR(10) NULL,
	DonGia FLOAT NULL,
	NgayAD DATETIME NULL,
	PRIMARY KEY (MaTA)
)

--************ Table Thu_ThucAn ************--
CREATE TABLE Thu_ThucAn
(
	MaThu NVARCHAR(10) NOT NULL,
	MaTA NVARCHAR(10) NOT NULL,
	SL INT NULL,
	FOREIGN KEY (MaThu) REFERENCES Thu(MaThu),
	FOREIGN KEY (MaTA) REFERENCES ThucAn(MaTA)
)

--ThuChuong
--INSERT INTO ThuChuong(MaChuong, MaThu, NgayVao, LyDoVao) VALUES (N'C0001', N'T0001', '2021-11-05', N'Cần bảo tồn')
--INSERT INTO ThuChuong(MaChuong, MaThu, NgayVao, LyDoVao) VALUES (N'C0002', N'T0001', '2011-12-05', N'Cần bảo tồn')
--INSERT INTO ThuChuong(MaChuong, MaThu, NgayVao, LyDoVao) VALUES (N'C0003', N'T0002', '2012-11-25', N'Cần bảo tồn')
--INSERT INTO ThuChuong(MaChuong, MaThu, NgayVao, LyDoVao) VALUES (N'C0004', N'T0003', '2020-01-05', N'Cần bảo tồn')

SELECT * FROM ThuChuong
--Chuong
INSERT INTO Chuong(MaChuong, MaTT, MaNV, MaLoai, MaKhu, DienTich, ChieuCao, SLThu ) VALUES(N'C0001', N'TT0001', N'NV0001', N'L0001', N'K0001', 20, 5, 0);
INSERT INTO Chuong(MaChuong, MaTT, MaNV, MaLoai, MaKhu, DienTich, ChieuCao, SLThu ) VALUES(N'C0002', N'TT0001', N'NV0002', N'L0001', N'K0001', 12, 3.5, 0);
INSERT INTO Chuong(MaChuong, MaTT, MaNV, MaLoai, MaKhu, DienTich, ChieuCao, SLThu ) VALUES(N'C0003', N'TT0003', N'NV0003', N'L0002', N'K0002', 32, 12, 0);
INSERT INTO Chuong(MaChuong, MaTT, MaNV, MaLoai, MaKhu, DienTich, ChieuCao, SLThu ) VALUES(N'C0004', N'TT0004', N'NV0004', N'L0003', N'K0003', 25, 5, 0);

SELECT * FROM Chuong 

--Khu
INSERT INTO Khu(MaKhu, TenKhu, GhiChu) VALUES(N'K0001', N'Động vật ăn cỏ', NULL)
INSERT INTO Khu(MaKhu, TenKhu, GhiChu) VALUES(N'K0002', N'Động vật hoang dã', NULL)
INSERT INTO Khu(MaKhu, TenKhu, GhiChu) VALUES(N'K0003', N'Động vật dưới nước', NULL)

SELECT * FROM Khu

--Loai
INSERT INTO Loai(MaLoai, TenLoai, GhiChu) VALUES(N'L0001', N'Động vật có vú', NULL)
INSERT INTO Loai(MaLoai, TenLoai, GhiChu) VALUES(N'L0002', N'Động vật bò sát', NULL)
INSERT INTO Loai(MaLoai, TenLoai, GhiChu) VALUES(N'L0003', N'Động vật lưỡng cư', NULL)

SELECT * FROM Loai


--LYDO
--INSERT INTO LyDo (MaLD, TenLD, GhiChu) VALUES('LD0001', N'Ốm', NULL)
--INSERT INTO LyDo (MaLD, TenLD, GhiChu) VALUES('LD0002', N'Chết', NULL)
--INSERT INTO LyDo (MaLD, TenLD, GhiChu) VALUES('LD0003', N'Mang bầu', NULL)
--INSERT INTO LyDo (MaLD, TenLD, GhiChu) VALUES('LD0004', N'Già', NULL)
--INSERT INTO LyDo (MaLD, TenLD, GhiChu) VALUES('LD0005', N'Mới Sinh', NULL);


--KHACPHUC
--INSERT INTO KhacPhuc (MaKP, TenCachKP, GhiChu) VALUES ('KP0001', N'Tiêm thuốc', NULL)
--INSERT INTO KhacPhuc (MaKP, TenCachKP, GhiChu) VALUES ('KP0002', N'Chôn cất', NULL)
--INSERT INTO KhacPhuc (MaKP, TenCachKP, GhiChu) VALUES ('KP0003', N'Xếp lịch khám', NULL)
--INSERT INTO KhacPhuc (MaKP, TenCachKP, GhiChu) VALUES ('KP0004', N'Tiêm kháng sinh', NULL)
--INSERT INTO KhacPhuc (MaKP, TenCachKP, GhiChu) VALUES ('KP0005', N'Đưa vào khu vực đặc biệt', NULL);

--SELECT * FROM KhacPhuc

--TRANGTHAI
INSERT INTO TrangThai (MaTT, TenTT, GhiChu) VALUES (N'TT0001', N'Trống', NULL)
INSERT INTO TrangThai (MaTT, TenTT, GhiChu) VALUES (N'TT0002', N'Đầy', NULL)
INSERT INTO TrangThai (MaTT, TenTT, GhiChu) VALUES (N'TT0003', N'Còn chỗ', NULL)
INSERT INTO TrangThai (MaTT, TenTT, GhiChu) VALUES (N'TT0004', N'Thừa', N'Thừa cá thể');

SELECT * FROM TrangThai

--NHANVIEN
INSERT INTO NhanVien (MaNV, TenNV, NTNS, GTinh, DiaChi, SDT, TenDN, MatKhau) VALUES (N'NV0001', N'Nguyễn Gia Phú', '1993-04-18', N'Nam', N'Hà Nội', '1111111111', N'phu', 123)
INSERT INTO NhanVien (MaNV, TenNV, NTNS, GTinh, DiaChi, SDT, TenDN, MatKhau) VALUES (N'NV0002', N'Nguyễn Thị Thùy Dương', '1990-07-30', N'Nữ', N'Hà Nội', '2222222222', N'tduong', 123)
INSERT INTO NhanVien (MaNV, TenNV, NTNS, GTinh, DiaChi, SDT, TenDN, MatKhau) VALUES (N'NV0003', N'Hoàng Dương', '1987-02-12', N'Nam', N'Hà Nội', '3333333333', N'hduong', 123)
INSERT INTO NhanVien (MaNV, TenNV, NTNS, GTinh, DiaChi, SDT, TenDN, MatKhau) VALUES (N'NV0004', N'Phạm Hữu Quang', '1996-06-28', N'Nam', N'Hà Nội', '4444444444', N'quang', 123)
INSERT INTO NhanVien (MaNV, TenNV, NTNS, GTinh, DiaChi, SDT, TenDN, MatKhau) VALUES (N'NV0005', N'Nguyễn Văn Tuyên', '1991-09-15', N'Nam', N'Hà Nội', '5555555555', N'tuyen', 123);

SELECT * FROM NhanVien

----Nguon goc
--INSERT INTO NguonGoc (TenNguonGoc, TenKieuSinh, GhiChu) VALUES (N'Châu Á', NULL, NULL)
--INSERT INTO NguonGoc (TenNguonGoc, TenKieuSinh, GhiChu) VALUES (N'Indoneia', NULL, NULL)
--INSERT INTO NguonGoc (TenNguonGoc, TenKieuSinh,  GhiChu) VALUES (N'Châu Phi', NULL, NULL) 
--INSERT INTO NguonGoc (TenNguonGoc, TenKieuSinh, GhiChu) VALUES (N'Châu Phi Hạ Saharai', NULL, NULL)
--INSERT INTO NguonGoc (TenNguonGoc, TenKieuSinh, GhiChu) VALUES (N'México', NULL, NULL)

--SELECT * FROM NguonGoc

--Thu
INSERT INTO THU (MaThu, TenNguonGoc, TenThu, MaLoai, SoLuong, SachDo, TenKhoaHoc, TenTA, TenTV, KieuSinh, NgayVao, DacDiem, NgaySinh, Anh, TuoiTho)
VALUES (N'T0001', N'Châu Á', N'Voi', 'L0001', 20, N'Có', N'Elephantidae', N'Elephas maximus', N'Voi Ấn Độ', N'Đẻ con', '2006-10-13', NULL, '2004-09-12', NULL, NULL)	
INSERT INTO THU (MaThu, TenNguonGoc, TenThu, MaLoai, SoLuong, SachDo, TenKhoaHoc, TenTA, TenTV, KieuSinh, NgayVao, DacDiem, NgaySinh, Anh, TuoiTho)
VALUES (N'T0002', N'Indoneia', N'Đười ươi', 'L0002', 12, N'Có', N'Pongo', N'Pongo pygmaeus', N'Đười ươi Borneo', N'Đẻ con', '2020-09-09', NULL, '2017-09-02', NULL, NULL)	
INSERT INTO THU (MaThu, TenNguonGoc, TenThu, MaLoai, SoLuong, SachDo, TenKhoaHoc, TenTA, TenTV, KieuSinh, NgayVao, DacDiem, NgaySinh, Anh, TuoiTho)
VALUES (N'T0003', N'Châu Phi', N'Sư tử', 'L0003', 7, N'Có', N'Felis leo', N'Lion', N'Sư tử', N'Đẻ con', '2021-12-01', NULL, '2016-07-12', NULL, NULL)	
INSERT INTO THU (MaThu, TenNguonGoc, TenThu, MaLoai, SoLuong, SachDo, TenKhoaHoc, TenTA, TenTV, KieuSinh, NgayVao, DacDiem, NgaySinh, Anh, TuoiTho)
VALUES (N'T0004', N'Châu Phi Hạ Saharai', N'Hà mã', 'L0001', 10, N'Có', N'Hippopotamus amphibius', N'hippo', N'Hà mã', N'Đẻ con', '2019-08-07', NULL, '2018-09-12', NULL, NULL)	

SELECT * FROM Thu

--Thuc an
INSERT INTO ThucAn (MaTA, Ten, CongDung, MaDV, DonGia, NgayAD) VALUES ('TA0001', N'Cỏ', NULL, NULL, 50000, NULL)
INSERT INTO ThucAn (MaTA, Ten, CongDung, MaDV, DonGia, NgayAD) VALUES ('TA0002', N'Thịt', NULL, NULL, 160000, NULL)
INSERT INTO ThucAn (MaTA, Ten, CongDung, MaDV, DonGia, NgayAD) VALUES ('TA0003', N'Trái cây', NULL, NULL, 120000, NULL)
INSERT INTO ThucAn (MaTA, Ten, CongDung, MaDV, DonGia, NgayAD) VALUES ('TA0004', N'Cá', NULL, NULL, 70000, NULL)
INSERT INTO ThucAn (MaTA, Ten, CongDung, MaDV, DonGia, NgayAD) VALUES ('TA0005', N'Hạt', NULL, NULL, 70000, NULL)

SELECT * FROM ThucAn

--Thu - Thuc an
INSERT INTO Thu_ThucAn(MaThu, MaTA, SL) VALUES (N'T0001', N'TA0001', 5)
INSERT INTO Thu_ThucAn(MaThu, MaTA, SL) VALUES (N'T0001', N'TA0004', 5)
INSERT INTO Thu_ThucAn(MaThu, MaTA, SL) VALUES (N'T0002', N'TA0003', 5)
INSERT INTO Thu_ThucAn(MaThu, MaTA, SL) VALUES (N'T0003', N'TA0002', 10)
INSERT INTO Thu_ThucAn(MaThu, MaTA, SL) VALUES (N'T0004', N'TA0002', 3)
INSERT INTO Thu_ThucAn(MaThu, MaTA, SL) VALUES (N'T0004', N'TA0004', 7)

SELECT * FROM Thu_ThucAn

--Su kien
INSERT INTO SuKien(MaSK, TenSK, GhiChu) VALUES (N'SK0001', N'Ốm', NULL)
INSERT INTO SuKien(MaSK, TenSK, GhiChu) VALUES (N'SK0002', N'Bỏ ăn', NULL)
INSERT INTO SuKien(MaSK, TenSK, GhiChu) VALUES (N'SK0003', N'Chết', NULL)
INSERT INTO SuKien(MaSK, TenSK, GhiChu) VALUES (N'SK0004', N'Bị thương', NULL)

SELECT * FROM SuKien

--Thu - Su kien
INSERT INTO Thu_SuKien(MaSK, MaThu, NgayBD, TenLD, CachKP, NgayKT) VALUES(N'SK0001', N'T0001', '2023-01-01', N'Ốm',  N'Xếp lịch khám', '2023-02-02')
INSERT INTO Thu_SuKien(MaSK, MaThu, NgayBD, TenLD, CachKP, NgayKT) VALUES(N'SK0004', N'T0002', '2023-05-01', N'Bị tai nạn', N'Đưa đi thú y', '2023-06-02')
INSERT INTO Thu_SuKien(MaSK, MaThu, NgayBD, TenLD, CachKP, NgayKT) VALUES(N'SK0003', N'T0003', '2023-04-02', N'Già',  N'Chôn cất', '2023-06-12')
INSERT INTO Thu_SuKien(MaSK, MaThu, NgayBD, TenLD, CachKP, NgayKT) VALUES(N'SK0002', N'T0004', '2023-11-01', N'Vấn đề đường tiêu hóa', N'Tiêm thuốc', '2023-11-15')

SELECT * FROM Thu_SuKien