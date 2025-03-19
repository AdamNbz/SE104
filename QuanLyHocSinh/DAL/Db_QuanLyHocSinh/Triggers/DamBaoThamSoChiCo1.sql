Create Trigger DamBaoThamSoChiCo1
On ThamSo
After Insert, Delete
As
Begin
	Declare @soluong TinyInt
	Select @soluong = Count(*) From ThamSo

	If(@soluong != 1)
	begin
		RaisError(N'ThamSo ph?i có 1 và ch? 1 d?ng!', 16, 1)
		Rollback Transaction
	end
End