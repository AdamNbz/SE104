Create Table HocSinh
(
	MaHS Char(6) Not Null,
	HoTen NVarChar(30),
	GioiTinh NVarChar(3),
	DiaChi NVarChar(100),
	Email VarChar(50),

	Constraint pk_HocSinh Primary Key(MaHS)
)
Go

Alter Table HocSinh Add NgaySinh DateTime
Go
Alter Table HocSinh Alter Column GioiTinh NVarChar(6)
Go

