using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaBot.Application.Common.Models.Wrappers;
public class EitherOne<TLeft, ТRight>
{
    private readonly TLeft _left;
    private readonly ТRight _right;
    public Object Current => this._isLeft ? this._left : this._right;
    private readonly bool _isLeft;
    public EitherOne(TLeft left)
    {
        _left = left;
        _isLeft = true;
    }
    public EitherOne(ТRight right)
    {
        _right = right;
        _isLeft = false;
    }
    public T Match<T>(Func<TLeft, T> left, Func<ТRight, T> right)
    {
        return _isLeft ? left(_left) : right(_right);
    }

    public static implicit operator EitherOne<TLeft, ТRight>(TLeft obj) =>
         new EitherOne<TLeft, ТRight>(obj);

    public static implicit operator EitherOne<TLeft, ТRight>(ТRight obj) =>
        new EitherOne<TLeft, ТRight>(obj);
}
