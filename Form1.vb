Public Class Form1
    Structure card
        Public intVal As Integer
        Public strLoc As String
        Public boolVisible As Boolean
    End Structure
    Private Cards(15) As card
    Private picCard() As PictureBox
    Private strLocation() As String = {"", "D:\Google Drive\Comp 9\Project 1\1.png", "D:\Google Drive\Comp 9\Project 1\2.png",
                                           "D:\Google Drive\Comp 9\Project 1\3.png", "D:\Google Drive\Comp 9\Project 1\4.png",
                                           "D:\Google Drive\Comp 9\Project 1\5.png", "D:\Google Drive\Comp 9\Project 1\6.png",
                                           "D:\Google Drive\Comp 9\Project 1\7.png", "D:\Google Drive\Comp 9\Project 1\8.png"}

    Private intFirst(1) As Integer
    Private intSecond(1) As Integer
    Private intState As Integer = 0
    Private intTurn As Integer = 0
    Private intFinish As Integer = 8
    Private intBScore(9) As Integer

    Private Sub shuffle()
        Dim randGen As New Random
        Dim intNumbers() As Integer = {1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8}
        Dim intCounter As Integer = 15
        picCard = {PictureBox1, PictureBox2, PictureBox3, PictureBox4, PictureBox5, PictureBox6, PictureBox7, PictureBox8,
                   PictureBox9, PictureBox10, PictureBox11, PictureBox12, PictureBox13, PictureBox14, PictureBox15, PictureBox16}
        Do While intCounter > -1
            Dim intNum As Integer = randGen.Next(0, intCounter + 1)
            Cards(intCounter).intVal = intNumbers(intNum)
            Cards(intCounter).strLoc = strLocation(intNumbers(intNum))
            Cards(intCounter).boolVisible = False
            For i As Integer = intNum To intCounter
                If i = intCounter Then
                    intNumbers(i) = 0
                Else
                    intNumbers(i) = intNumbers(i + 1)
                End If

            Next
            intCounter -= 1
        Loop
    End Sub
    Private Sub bestScore()
        Dim score As Integer = intTurn + 1
        Dim current As Integer
        For i As Integer = 0 To 9
            current = intBScore(i)
            If current <= 8 Then
                current = 9999999
            End If
            If score >= 8 AndAlso score < current Then
                intBScore(i) = score
                score = current
            End If
        Next i
        writeToFile()
    End Sub
    Private Function showBest() As String
        Dim strBest As String = ""
        For i As Integer = 0 To 9
            If intBScore(i) >= 8 Then
                strBest &= intBScore(i).ToString & Environment.NewLine
            End If
        Next i
        Return strBest

    End Function
    Private Sub writeToFile()
        Dim outFile As IO.StreamWriter = IO.File.CreateText("BestScore.txt")
        For intIndex As Integer = 0 To 9
            outFile.Write(String.Empty)
            outFile.WriteLine(intBScore(intIndex))
        Next intIndex
        outFile.Close()
    End Sub
    Private Sub reset()
        lblScore.Text = 0
        intState = 0
        intFinish = 8
        intTurn = 0
        shuffle()
        For i As Integer = 0 To 15
            picCard(i).Image = Image.FromFile(Cards(i).strLoc)
            picCard(i).Visible = Cards(i).boolVisible
        Next i
    End Sub
    Private Sub test(i As Integer)
        lblScore.Text = intTurn.ToString
        If intState = 0 Then
            intFirst(0) = Cards(i).intVal
            intFirst(1) = i
            intState = 1
            intTurn += 1
        ElseIf intState = 1 And Not Cards(i).boolVisible Then
            intSecond(0) = Cards(i).intVal
            intSecond(1) = i
            intState = 2
        ElseIf intState = 2 And Not Cards(i).boolVisible Then
            If intFirst(0) <> intSecond(0) Then
                Cards(intFirst(1)).boolVisible = False
                picCard(intFirst(1)).Visible = False
                Cards(intSecond(1)).boolVisible = False
                picCard(intSecond(1)).Visible = False
            Else
                intFinish -= 1
                If intFinish = 1 Then
                    bestScore()
                    lblBestScore.Text = showBest()
                End If
            End If
            intFirst(0) = Cards(i).intVal
            intFirst(1) = i
            intState = 1
            intTurn += 1
        End If
        Cards(i).boolVisible = True
        picCard(i).Visible = True
    End Sub
    Private Sub btnNewGame_Click(sender As Object, e As EventArgs) Handles btnNewGame.Click
        reset()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        reset()
        Dim inFile As IO.StreamReader
        If IO.File.Exists("BestScore.txt") Then
            inFile = IO.File.OpenText("BestScore.txt")
            For intIndex As Integer = 0 To 9
                intBScore(intIndex) = inFile.ReadLine
            Next intIndex
            inFile.Close()
        End If
        lblBestScore.Text = showBest()
    End Sub

    Private Sub btnHighScore_Click(sender As Object, e As EventArgs) Handles btnHighScore.Click
        MessageBox.Show("Best Scores:" & Environment.NewLine & showBest(), "Memory Game", MessageBoxButtons.OK)

    End Sub

    Private Sub pic1_Click(sender As Object, e As EventArgs) Handles pic1.Click, pic2.Click, pic3.Click, pic4.Click, pic5.Click, pic6.Click, pic7.Click, pic8.Click, pic9.Click, pic10.Click, pic11.Click, pic12.Click, pic13.Click, pic14.Click, pic15.Click, pic16.Click
        Dim pic As PictureBox = DirectCast(sender, PictureBox)
        Dim i As Integer = pic.Tag
        test(i)
    End Sub


End Class
