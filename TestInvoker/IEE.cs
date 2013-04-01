using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestInvoker
{
	static class IEE
	{
		public static void ForEach<T>(this IEnumerable<T> souce, Action<T> f)
		{
			foreach (var item in souce)
			{
				f(item);
				
			}
		}
		public static IEnumerable<T> Do<T>(this IEnumerable<T> souce,Action<T> f)
		{
			foreach (var item in souce)
			{
				f(item);
				yield return item;
			}
		}

		public static IEnumerable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
		{
			TState arg = initialState;

			while (condition(arg))
			{
				yield return resultSelector(arg);
				arg = iterate(arg);
			}
		}
		//public static IEnumerable<TResult> Generate<T,TResult>(T ini, Predicate<TResult> cond, Func<T, TResult> cont,Func<T,T> cont2)
		//{
		//    //var next=cont2(ini);
		//    //if (cond(cont(ini))) yield return cont(ini); Generate<T,TResult>(next, cond, cont, cont2);
		//    var i = cont(ini);
		//    var i2 = cont2(ini);
		//    while(cond(i))
		//    {
		//        i2 = cont2(i2);
		//        yield return i;
				
		//        i = cont(i2);
		//    }//Generate(0, i => i.Length < 20, i => i.tostring,_=>_+1)
		//    yield return i;
		//    //if(cond(i))Generate()
		//}
		public static IEnumerable<TResult> Pick<T,TResult>(this IEnumerable<T> souce,Func<T,TResult> f){
			foreach (var i in souce)
			{
				TResult ret;
				try
				{
					ret = f(i);
				}
				catch (Exception)
				{
					continue;
				}
				yield return ret;
			}
		}
	}
}
