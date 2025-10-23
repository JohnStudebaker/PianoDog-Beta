Imports System.Collections.Generic
Imports System.Linq

Public Class AnalogSmoother
    ' Use a Queue to store the last 5 samples. It's efficient for this.
    Private ReadOnly samples As New Queue(Of Double)
    Private Const WindowSize As Integer = 5

    ''' <summary>
    ''' Adds a new sample and returns the smoothed value using a Median Filter.
    ''' </summary>
    ''' <param name="newSample">The latest raw reading from the ADC.</param>
    ''' <returns>The smoothed (median) value.</returns>
    Public Function AddSampleAndGetMedian(ByVal newSample As Double) As Double
        ' Add the new sample to the queue
        samples.Enqueue(newSample)

        ' If the queue is larger than our window size, remove the oldest sample
        If samples.Count > WindowSize Then
            samples.Dequeue()
        End If

        ' To get the median, we first need to sort the values.
        ' Convert the queue to a list to make it sortable.
        Dim sortedSamples As List(Of Double) = samples.ToList()
        sortedSamples.Sort()

        ' Calculate the middle index. Integer division handles this for us.
        Dim midIndex As Integer = sortedSamples.Count / 2

        ' Return the middle value (the median)
        ' Note: This handles both even and odd window sizes correctly for our purpose.
        Return sortedSamples(midIndex)
    End Function

    ''' <summary>
    ''' This shows your original moving average method for comparison.
    ''' </summary>
    ''' <returns>The average value.</returns>
    Public Function GetAverage() As Double
        If samples.Count = 0 Then
            Return 0.0
        End If
        ' The .Average() LINQ extension makes this very easy.
        Return samples.Average()
    End Function
End Class

' --- How to use it in your code ---
' Dim smoother As New AnalogSmoother()
'
' ' In your timer or data reading loop:
' Dim rawValue As Double = ReadAnalogInput() ' Your function to get the raw value
' Dim smoothedValue As Double = smoother.AddSampleAndGetMedian(rawValue)
'
' ' Now use 'smoothedValue' in the rest of your program
' Console.WriteLine($"Raw: {rawValue}, Smoothed: {smoothedValue}")