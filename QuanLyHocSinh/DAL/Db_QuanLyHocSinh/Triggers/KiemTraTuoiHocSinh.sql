Create Trigger KiemTraTuoiHocSinh
On HocSinh
After Insert, Update
As
Begin
	Declare @tuoitoithieu TinyInt, @tuoitoida TinyInt

	Select @tuoitoithieu = ts.TuoiToiThieu, @tuoitoida = ts.TuoiToiDa From ThamSo ts

	If Exists (
				Select *
				From inserted i
				Where i.NgaySinh Not Between DateAdd(Year, -@tuoitoida, GetDate()) And DateAdd(Year, -@tuoitoithieu, GetDate())
				)
	begin
		RaisError(N'Có h?c sinh v?i ð? tu?i không h?p l?', 16, 1)
		Rollback Transaction
	end
End