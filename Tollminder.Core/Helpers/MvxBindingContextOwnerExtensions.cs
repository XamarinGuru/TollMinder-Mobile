using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings;

namespace Tollminder.Core.Helpers
{
	public static class MvxBindingContextOwnerExtensions
	{
		// add whenever something changes
		public static void AddLinqBinding<TViewModel, TProperty>(
			this IMvxBindingContextOwner owner,
			TViewModel viewModel,
			Expression<Func<TViewModel, TProperty>> propertyExpression,
			Action<TProperty> action) where TViewModel : INotifyPropertyChanged
		{
			//Action createBinding = () =>
			//    {
			var propertyInfo = GetPropertyInfoFromExpression<TViewModel, TProperty>(propertyExpression);
			var binding = new LinqBinding<TViewModel, TProperty>(viewModel, propertyInfo, action);
			owner.AddBinding(owner, binding);
			//owner.CreateBinding(binding);
			//    };

			//viewModel.PropertyChanged += (sender, args) => 
			//    createBinding();

			//createBinding();
		}

		// add whenever something changes
		public static void AddLinqBinding<TViewModel, TProperty>(
			this IMvxBindingContextOwner owner,
			TViewModel viewModel,
			Expression<Func<TViewModel, TProperty>> propertyExpression,
			Action action) where TViewModel : INotifyPropertyChanged
		{
			AddLinqBinding(owner, viewModel, propertyExpression, (TProperty v) => action());
		}

		// get property info from an expression
		private static PropertyInfo GetPropertyInfoFromExpression<TViewModel, TProperty>(
			Expression<Func<TViewModel, TProperty>> expression)
		{
			Type baseType = typeof(TViewModel);

			// get the member info
			MemberExpression member = expression.Body as MemberExpression;
			if (member == null)
				throw new ArgumentException(string.Format(
					"Expression '{0}' refers to a method, not a property.", expression));

			// get the property info
			PropertyInfo propInfo = member.Member as PropertyInfo;
			if (propInfo == null)
				throw new ArgumentException(
					string.Format("Expression '{0}' refers to a field, not a property.",
						expression));

			/*
            // .NET 4.0 version
            if (baseType != propInfo.ReflectedType && !baseType.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(
                    string.Format("Expresion '{0}' refers to a property that is not from type {1}.",
                        expression, baseType));
            */

			// check the base is the right type
			var firstParameterType = expression.Parameters.First().Type;
			if (baseType != firstParameterType && !firstParameterType.GetTypeInfo().IsSubclassOf(baseType))
				throw new ArgumentException(
					string.Format("Expresion '{0}' refers to a property that is not from type {1}.",
						expression, baseType));

			// give back our work
			return propInfo;
		}

		// straight up LINQ binding to a property
		private class LinqBinding<TViewModel, TProperty> : IMvxUpdateableBinding
			where TViewModel : INotifyPropertyChanged
		{
			private TViewModel _ViewModel;
			private Action<TProperty> _OnUpdate;
			private PropertyInfo _PropertyInfo;

			// set binding action
			public LinqBinding(TViewModel viewModel, PropertyInfo propertyInfo, Action<TProperty> action)
			{
				_ViewModel = viewModel;
				_OnUpdate = action;
				_PropertyInfo = propertyInfo;

				// set event and kick
				_ViewModel.PropertyChanged += OnPropertyChanged;

				// kick once
				var propertyValue = (TProperty)propertyInfo.GetValue(viewModel, null);
				action(propertyValue);
			}

			// no idea what this is for... let's just play along
			private object _DataContext;
			public object DataContext
			{
				get
				{
					return _DataContext;
				}
				set
				{
					_DataContext = value;
				}
			}

			// remove handler
			public void Dispose()
			{
				_ViewModel.PropertyChanged -= OnPropertyChanged;
			}

			// run action on update, for the correct property
			private void OnPropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
			{
				if (eventArgs.PropertyName == _PropertyInfo.Name)
				{
					var propertyValue = (TProperty)_PropertyInfo.GetValue(_ViewModel, null);
					_OnUpdate(propertyValue);
				}
			}
		}
	}
}

