Public Class Form1
    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            'SerialPort1.PortName = cmbCOM.SelectedItem.ToString '設定PortName的值
            SerialPort1.ReceivedBytesThreshold = 1
            SerialPort1.ReadTimeout = 1000
            SerialPort1.Open() '開啟SerialPort
            Label7.Text = "已連線"
        Catch ex As Exception
            Label7.Text = ex.ToString()
        End Try

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SerialPort1.Close() '關閉SerialPort
        Label7.Text = "斷線中"
    End Sub

    Private Sub SerialPort1_DataReceived(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        Dim buff() As Byte
        Dim s As String = ""
        Dim j As Integer

        ReDim buff(SerialPort1.BytesToRead - 1)
        SerialPort1.Read(buff, 0, buff.Length)
        For i As Integer = 0 To buff.Length - 1
            s += buff(i).ToString("X2")
            If j < 22 Then
                j = j + 1
            End If
        Next
        Dim returnStr As String = s & vbCrLf
        'Return returnStr
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'Dim Output As String = (":" + "01030000" + LRC("010300000001") + Chr(13) + Chr(10))
        'Label5.Text = SerialPort1.Encoding("get") 'As System.Text.Encoding
        'TextBox2.Text = TextBox2.Text & s & vbCrLf
        Dim tString As String = "01030000001"
        'Dim tString As String = "01039C410002"
        'Dim tString As String = "010675310002"
        Dim TempString As String = ":" + tString + LRC(tString) + Chr(13) + Chr(10)
        ModbusWrite(TempString)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        'AxMSComm1.Output = ":" + "010300020002" + LRC("010300020002") + Chr(13) + Chr(10)
        'Dim tString As String = AxMSComm1.Input
        Dim tString As String = "01030000001"
        'Dim tString As String = "01039C410002"
        'Dim tString As String = "010375310002"
        Dim TempString As String = ":" + tString + LRC(tString) + Chr(13) + Chr(10)
        SerialPort1.Write(TempString)
        'SerialPort1.Read(TempString)
        'ModbusWrite(TempString)
        TempString = ModbusRead() '= SerialPort1.ReadExisting()
        Label7.Text = TempString
    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Public Function ModbusWrite(str As String) As String
        Dim l As Integer
        Dim buf As Byte()
        l = Len(str)
        buf = SerialPort1.Encoding.ASCII.GetBytes(str)
        'SerialPort1.Write(buf, 0, l)
        SerialPort1.Write(str)
    End Function

    Public Function ModbusRead() As String
        Dim str As String
        Dim daBUF() As Byte
        Dim n As Integer, i As Integer
        SerialPort1.ReadTimeout = 1000
        n = SerialPort1.BytesToRead
        If n > 0 Then
            ReDim daBUF(n - 1)
            n = SerialPort1.Read(daBUF, 0, SerialPort1.BytesToRead)
            '// 以下列函數將取得之陣列資料轉為字串格式
            str = SerialPort1.Encoding.ASCII.GetString(daBUF)

            '// 或使用下列程序將取得之陣列資料轉為字串格式
            'str = ""
            'For i = 0 To n - 1
            'str = str & Chr(Val(daBUF(i)))
            'Next
            '// 提示資料
            'txtRecv.Text = str
            Return str
        End If
    End Function

    Public Function LRC(str As String) As String
        Dim c As Integer = 0
        Dim l As Integer = Len(str)
        Dim c_data As String
        Dim d_lrc As String = ""
        Dim h_lrc As String

        For c = c + 1 To l
            c_data = Mid(str, c, 2)
            d_lrc = d_lrc + TxtToHex(c_data)
            c = c + 1
        Next c
        If d_lrc > &HFF Then
            d_lrc = d_lrc Mod &H100
        End If
        h_lrc = Hex(&HFF - d_lrc + 1)
        If Len(h_lrc) > 2 Then
            h_lrc = Mid(h_lrc, Len(h_lrc) - 1, 2)
        End If
        LRC = h_lrc
    End Function

    Public Function TxtToHex(ByVal s As String) As String
        Dim sstr As String, l As Integer, ss As String
        sstr = ""
        For l = 1 To Len(s)
            ss = Hex(Asc(Mid(s, l, 1)))
            If Len(ss) = 1 Then ss = "0" & ss
            sstr = sstr & ss
        Next
        TxtToHex = sstr
    End Function

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub
End Class