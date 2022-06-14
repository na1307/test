Public Class MainClass
    Public Shared Sub Main()
        Try
            Dim stb1 As New Strawberry("딸기1")
            Dim swp1 As New SweetPepper("피망1")

            Dim minsoo As New Human("민수", Gender.Male)

            With minsoo
                .SpeakName()
                .Eat(stb1)
                .Eat(swp1)
                .Sleep()
            End With

            Console.WriteLine()

            Dim stb2 As New Strawberry("딸기2")
            Dim swp2 As New SweetPepper("피망2")

            Dim zzanggu As New Human("짱구", Gender.Male, swp1)

            With zzanggu
                .SpeakName()
                .Eat(stb2)
                .Eat(swp2)
                .Sleep()
            End With

        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub
End Class

Public MustInherit Class Creature : Implements INameable
    Private ReadOnly _gender As Gender

    Public Sub New(argname As String, arggender As Gender)
        Name = If(argname, "(이름 없음)")
        _gender = arggender
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

    Public Sub New(name As String)
        Me.New(name, Gender.None)
    End Sub

    Public Sub New(gender As Gender)
        Me.New(Nothing, gender)
    End Sub

    Public Sub New()
        Me.New(Nothing, Gender.None)
    End Sub
End Class

Public MustInherit Class PlantFood : Inherits Plant : Implements IFood
    Private _IsEaten As Boolean

    Public Sub New(name As String, gender As Gender)
        MyBase.New(name, gender)
        _IsEaten = False
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

    Public MustOverride ReadOnly Property FoodName As String Implements IFood.FoodName
    Public MustOverride ReadOnly Property FoodTaste As Flavor Implements IFood.FoodTaste

    Public ReadOnly Property IsEaten As Boolean Implements IFood.IsEaten
        Get
            Return _IsEaten
        End Get
    End Property

    Public Function Eat() As Integer Implements IFood.Eat
        If IsEaten Then Throw New AlreadyEatenException(Me)

        _IsEaten = True

        Return 0
    End Function

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

    Public Sub New(name As String)
        Me.New(name, Gender.None)
    End Sub

    Public Sub New(gender As Gender)
        Me.New(Nothing, gender)
    End Sub

    Public Sub New()
        Me.New(Nothing, Gender.None)
    End Sub
End Class

Public MustInherit Class Fruit : Inherits PlantFood
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

    Public Sub New(name As String)
        Me.New(name, Gender.None)
    End Sub

    Public Sub New(gender As Gender)
        Me.New(Nothing, gender)
    End Sub

    Public Sub New()
        Me.New(Nothing, Gender.None)
    End Sub

    Public MustOverride Function Sleep() As Integer Implements ISleep.Sleep
    Public MustOverride Function Eat(food As IFood) As Integer Implements IEat.Eat
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
            i += 1
        Next

        Hates = temp
    End Sub

    Public ReadOnly Property Hates As String()

    Public Sub SpeakName() Implements ISpeak.SpeakName
        Console.WriteLine("내 이름은 " & Name)
    End Sub

    Public Overrides Function Sleep() As Integer
        Console.WriteLine("zzz...")

        Return 0
    End Function

    Public Overrides Function Eat(food As IFood) As Integer
        If Hates IsNot Nothing AndAlso Hates.Contains(food.FoodName) Then Throw New HateFoodException(food.FoodName)

        food.Eat()

        Console.WriteLine(food.FoodName & " 아이 맛있어")

        Return 0
    End Function

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
    Function Eat() As Integer
End Interface

Public Interface IEat
    Function Eat(food As IFood) As Integer
End Interface

Public Interface ISleep
    Function Sleep() As Integer
End Interface

Public Interface ISpeak
    Sub SpeakName()
End Interface