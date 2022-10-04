//1
/*Console.WriteLine(Add(4, 5));
Console.WriteLine(Add(4, 5));

MathOp fx = Add;
Console.WriteLine(fx(4, 5));
fx = Sub;
Console.WriteLine(fx(4, 5));*/

//2
ShowCalc(4, 5, delegate (int x, int y) { return x + y; });
ShowCalc(4, 5, (int x, int y) => { return x + y; });
ShowCalc(4, 5, (int x, int y) => x + y);
ShowCalc(4, 5, (x, y) => x + y);    //pas besoin de spécifier le type car il est connu grâçe au délégué
//pas besoin de spécifier le type juste après ShowCalc avec les <> parce que le type est déjà connu

int Add(int x, int y)
{
    return x + y;
}

int Sub(int x, int y)
{
    return x - y;
}

int Mult(int x, int y)
{
    return x * y;
}

int Div(int x, int y)
{
    return x / y;
}

void ShowCalc<T>(T x, T y, Combine<T> fx)
{
    T result = fx(x, y); // T peut être remplacé par var
    Console.WriteLine("" + result);
}

//delegate: variable qui a un pointeur vers une fonction
//delegate en bas sinon ça casse
//même signature (nb param, types de param et type de return)
//delegate int MathOp(int x, int y);

//signature: delegate a le même nb de param, les deux param ont les mêmes types et le type de return est le même
delegate T Combine<T>(T x, T y); //Generic pour avoir n'importe quelle type de variable, pas obligé d'être T mais toutes les lettres ne
                                 //fonctionnent pas