Imports System.Console

Module Main
    Public Sub Main()
        Try
            Dim instance = Container.Instance

            With instance
                .AddHuman(New Human("민수", Gender.Male))
                .AddHuman(New Human("짱구", Gender.Male, New SweetPepper))

                .AddFood(New Strawberry)
                .AddFood(New SweetPepper)
            End With

            Dim minsoo = instance.GetHuman(0)
            Dim zzanggu = instance.GetHuman(1)

            minsoo.SpeakName()
            zzanggu.SpeakName()

            minsoo.Eat(instance.GetFood(0))
            zzanggu.Eat(instance.GetFood(1))
        Catch ex As Exception : WriteLine(ex.Message)
        End Try
    End Sub
End Module

Public NotInheritable Class Container
    Private humans As Human()
    Private hi As Integer = 0
    Private foods As IFood()
    Private fi As Integer = 0

    Public Shared ReadOnly Property Instance As New Container

    Public Sub AddHuman(human As Human)
        ReDim Preserve humans(hi)
        humans.SetValue(human, hi)
        hi += 1
    End Sub

    Public Function GetHuman(index As Integer) As Human
        Return humans(index)
    End Function

    Public Sub AddFood(food As IFood)
        ReDim Preserve foods(fi)
        foods.SetValue(food, fi)
        fi += 1
    End Sub

    Public Function GetFood(index As Integer) As IFood
        Return foods(index)
    End Function
End Class

Public MustInherit Class Creature : Implements INameable
    Private ReadOnly _gender As Gender

    Public Sub New(argname As String, arggender As Gender)
        Name = If(argname, "(이름 없음)")
        _gender = arggender
    End Sub

    Public ReadOnly Property Name As String Implements INameable.Name

    Public ReadOnly Property GetGender As String
        Get
            Select Case _gender
                Case Gender.None
                    Return "Undefined"

                Case Gender.Male
                    Return "Male"

                Case Gender.Female
                    Return "Female"

                Case Else
                    Throw New NotSupportedException
            End Select
        End Get
    End Property
End Class

Public MustInherit Class Plant : Inherits Creature
    Public Sub New(name As String, gender As Gender)
        MyBase.New(name, gender)
    End Sub
End Class

Public MustInherit Class PlantFood : Inherits Plant : Implements IFood
    Private _IsEaten As Boolean

    Public Sub New(name As String, gender As Gender)
        MyBase.New(name, gender)
        _IsEaten = False
    End Sub

    Public MustOverride ReadOnly Property FoodName As String Implements IFood.FoodName
    Public MustOverride ReadOnly Property FoodTaste As Flavor Implements IFood.FoodTaste

    Public ReadOnly Property IsEaten As Boolean Implements IFood.IsEaten
        Get
            Return _IsEaten
        End Get
    End Property

    Public Sub Eat() Implements IFood.Eat
        If IsEaten Then Throw New AlreadyEatenException(Me)

        _IsEaten = True
    End Sub

    Private Class AlreadyEatenException : Inherits Exception
        Public Sub New(food As IFood)
            MyBase.New(food.Name & "은(는) 이미 먹어서 없다.")
        End Sub

        Public Sub New(food As IFood, inner As Exception)
            MyBase.New(food.Name & "은(는) 이미 먹어서 없다.", inner)
        End Sub
    End Class
End Class

Public MustInherit Class Vegetable : Inherits PlantFood
    Public Sub New(name As String, gender As Gender)
        MyBase.New(name, gender)
    End Sub
End Class

Public MustInherit Class Fruit : Inherits PlantFood
    Public Sub New(name As String, gender As Gender)
        MyBase.New(name, gender)
    End Sub
End Class

Public Class Strawberry : Inherits Fruit
    Public Sub New(name As String, gender As Gender)
        MyBase.New(name, gender)
    End Sub

    Public Sub New(name As String)
        Me.New(name, Gender.None)
    End Sub

    Public Sub New(gender As Gender)
        Me.New(Nothing, gender)
    End Sub

    Public Sub New()
        Me.New(Nothing, Gender.None)
    End Sub

    Public Overrides ReadOnly Property FoodName As String = "딸기"
    Public Overrides ReadOnly Property FoodTaste As Flavor = Flavor.Sweet
End Class

Public Class SweetPepper : Inherits Vegetable
    Public Sub New(name As String, gender As Gender)
        MyBase.New(name, gender)
    End Sub

    Public Sub New(name As String)
        Me.New(name, Gender.None)
    End Sub

    Public Sub New(gender As Gender)
        Me.New(Nothing, gender)
    End Sub

    Public Sub New()
        Me.New(Nothing, Gender.None)
    End Sub

    Public Overrides ReadOnly Property FoodName As String = "피망"
    Public Overrides ReadOnly Property FoodTaste As Flavor = Flavor.Spicy
End Class

Public MustInherit Class Animal : Inherits Creature : Implements ISleep, IEat
    Public Sub New(name As String, gender As Gender)
        MyBase.New(name, gender)
    End Sub

    Public MustOverride Sub Sleep() Implements ISleep.Sleep
    Public MustOverride Sub Eat(food As IFood) Implements IEat.Eat
End Class

Public Class Human : Inherits Animal : Implements ISpeak
    Public Sub New(name As String, gender As Gender)
        MyBase.New(name, gender)
    End Sub

    Public Sub New(name As String, gender As Gender, ParamArray hatefoods As IFood())
        Me.New(name, gender)

        Dim i = 0US
        Dim temp(i) As String

        For Each hatefood In hatefoods
            ReDim Preserve temp(i)
            temp.SetValue(hatefood.FoodName, i)
            i += 1US
        Next

        Hates = temp
    End Sub

    Public ReadOnly Property Hates As String()

    Public Sub SpeakName() Implements ISpeak.SpeakName
        WriteLine("내 이름은 " & Name)
    End Sub

    Public Overrides Sub Sleep()
        WriteLine("zzz...")
    End Sub

    Public Overrides Sub Eat(food As IFood)
        If Hates IsNot Nothing AndAlso Hates.Contains(food.FoodName) Then Throw New HateFoodException(food.FoodName)

        food.Eat()

        WriteLine(food.FoodName & " 아이 맛있어")
    End Sub

    Private Class HateFoodException : Inherits Exception
        Public Sub New(food As String)
            MyBase.New("으악! " & food & "은(는) 싫어!")
        End Sub

        Public Sub New(food As String, inner As Exception)
            MyBase.New("으악! " & food & "은(는) 싫어!", inner)
        End Sub
    End Class
End Class

Public Enum Gender
    None = Nothing
    Male
    Female
End Enum

Public Enum Flavor
    Sweet
    Spicy
    Salty
    Sour
    Bitter
End Enum

Public Interface INameable
    ReadOnly Property Name As String
End Interface

Public Interface IFood : Inherits INameable
    ReadOnly Property FoodName As String
    ReadOnly Property FoodTaste As Flavor
    ReadOnly Property IsEaten As Boolean
    Sub Eat()
End Interface

Public Interface IEat
    Sub Eat(food As IFood)
End Interface

Public Interface ISleep
    Sub Sleep()
End Interface

Public Interface ISpeak
    Sub SpeakName()
End Interface