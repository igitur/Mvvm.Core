﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Nito.Mvvm
{
    /// <summary>
    /// A subscription to a property changed event for a particular property.
    /// </summary>
    public sealed class PropertyChangedSubscription: IDisposable
    {
        /// <summary>
        /// The object whose property is observed. Never <c>null</c>.
        /// </summary>
        private readonly INotifyPropertyChanged _source;

        /// <summary>
        /// The name of the property being observed. May be <c>null</c>.
        /// </summary>
        private readonly string _propertyName;

        /// <summary>
        /// The callback to invoke when the property changed. Never <c>null</c>.
        /// </summary>
        private readonly PropertyChangedEventHandler _handler;

        /// <summary>
        /// The actual subscription to <see cref="INotifyPropertyChanged.PropertyChanged"/>. Never <c>null</c>.
        /// </summary>
        private readonly PropertyChangedEventHandler _subscription;

        /// <summary>
        /// Subscribes to the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a particular property.
        /// </summary>
        /// <param name="source">The object whose property is observed. May not be <c>null</c>.</param>
        /// <param name="propertyName">The name of the property to observe. May be <c>null</c> to indicate that all properties should be observed.</param>
        /// <param name="handler">The callback that is called when the property has changed. May not be <c>null</c>.</param>
        public PropertyChangedSubscription(INotifyPropertyChanged source, string propertyName, PropertyChangedEventHandler handler)
        {
            _source = source;
            _propertyName = propertyName;
            _handler = handler;

            _subscription = SourceOnPropertyChanged;
            _source.PropertyChanged += _subscription;
        }

        /// <summary>
        /// Subscribes to the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a particular property.
        /// </summary>
        /// <param name="source">The object whose property is observed. May not be <c>null</c>.</param>
        /// <param name="propertyName">The name of the property to observe. May be <c>null</c> to indicate that all properties should be observed.</param>
        /// <param name="handler">The callback that is called when the property has changed. May not be <c>null</c>.</param>
        public PropertyChangedSubscription(INotifyPropertyChanged source, string propertyName, Action handler)
            : this(source, propertyName, (_, __) => handler())
        {
        }

        private void SourceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == _propertyName || _propertyName == null)
                _handler(sender, propertyChangedEventArgs);
        }

        /// <summary>
        /// Subscribes to the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a particular property.
        /// </summary>
        /// <param name="source">The object whose property is observed. May not be <c>null</c>.</param>
        /// <param name="propertyName">The name of the property to observe. May be <c>null</c> to indicate that all properties should be observed.</param>
        /// <param name="handler">The callback that is called when the property has changed. May not be <c>null</c>.</param>
        /// <returns>The new subscription.</returns>
        public static IDisposable Create(INotifyPropertyChanged source, string propertyName, PropertyChangedEventHandler handler)
        {
            return new PropertyChangedSubscription(source, propertyName, handler);
        }

        /// <summary>
        /// Subscribes to the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a particular property.
        /// </summary>
        /// <param name="source">The object whose property is observed. May not be <c>null</c>.</param>
        /// <param name="propertyName">The name of the property to observe. May be <c>null</c> to indicate that all properties should be observed.</param>
        /// <param name="handler">The callback that is called when the property has changed. May not be <c>null</c>.</param>
        /// <returns>The new subscription.</returns>
        public static IDisposable Create(INotifyPropertyChanged source, string propertyName, Action handler)
        {
            return new PropertyChangedSubscription(source, propertyName, handler);
        }

        /// <summary>
        /// Unsubscribes from the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        public void Dispose()
        {
            _source.PropertyChanged -= _subscription;
        }
    }
}