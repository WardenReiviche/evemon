﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.XPath;
using EVEMon.Common.Service;

namespace EVEMon.Common.MarketPricer
{
    public abstract class ItemPricer
    {
        protected static readonly Dictionary<int, double> PriceByItemID = new Dictionary<int, double>();

        protected static DateTime CachedUntil;

        protected static string SelectedProviderName;
        protected static bool Loaded;

        /// <summary>
        /// Gets the name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <value>
        /// The providers.
        /// </value>
        public static IEnumerable<ItemPricer> Providers
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetTypes()
                    .Where(type => typeof(ItemPricer).IsAssignableFrom(type) && type.GetConstructor(Type.EmptyTypes) != null)
                    .Select(type => Activator.CreateInstance(type) as ItemPricer)
                    .Where(provider => !String.IsNullOrWhiteSpace(provider.Name))
                    .OrderBy(pricer => pricer.Name);
            }
        }

        /// <summary>
        /// Gets the price by type ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public abstract double GetPriceByTypeID(int id);

        /// <summary>
        /// Gets the prices asynchronous.
        /// </summary>
        /// Gets the item prices list.
        protected abstract void GetPricesAsync();
        
        /// <summary>
        /// Saves the xml document to the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="xdoc">The xdoc.</param>
        protected static void Save(string filename, IXPathNavigable xdoc)
        {
            LocalXmlCache.Save(filename, xdoc);
        }
    }
}