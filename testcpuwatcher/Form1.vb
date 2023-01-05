Imports System.ComponentModel
Imports System.Threading
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows
Imports System.Windows.Input
Public Class Form1
    Private worker As BackgroundWorker
    Private cpuUsageCounter As PerformanceCounter
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0
        Me.Left = 0

        For Each c As PerformanceCounterCategory In PerformanceCounterCategory.GetCategories()
            If c.CategoryName = "Processor" Then
                For Each pc As PerformanceCounter In c.GetCounters("_Total")

                    If pc.CounterName = "% Processor Time" Then
                        Me.cpuUsageCounter = pc

                        Console.WriteLine("CategoryName:{0:s}", pc.CategoryName)
                        Console.WriteLine("CounterName:{0:s}", pc.CounterName)
                        Console.WriteLine("InstanceName:{0:s}", pc.InstanceName)

                        Exit For
                    End If
                Next pc
            End If
        Next c

        worker = New BackgroundWorker()
        AddHandler worker.DoWork, AddressOf BackgroundWorker1_DoWork
        AddHandler worker.ProgressChanged, AddressOf Worker_ProgressChanged
        worker.WorkerReportsProgress = True
        worker.RunWorkerAsync()
    End Sub
    Private Sub Worker_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs)
        labelCpuUsage.Text = String.Format("{0:d}%", e.ProgressPercentage)
    End Sub
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Do

            If worker.CancellationPending Then
                Return
            End If

            Thread.Sleep(1000)

            If Me.cpuUsageCounter IsNot Nothing Then
                Dim value As Single = cpuUsageCounter.NextValue()
                Console.WriteLine(String.Format("現在のCPU使用率は {0:f}% です。", value))
                worker.ReportProgress(CInt(Math.Truncate(value)))
            End If

        Loop
    End Sub
End Class
