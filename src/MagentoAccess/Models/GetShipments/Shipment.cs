﻿using System;
using System.Collections.Generic;
using System.Linq;
using MagentoAccess.Models.Services.Rest.v2x.SalesOrderRepository;

namespace MagentoAccess.Models.GetShipments
{
	public class Shipment
	{
		public string Id { get; set; }
		public string OrderId { get; set; }
		public string OrderIncrementId { get; set; }
		public DateTime CreatedAtUtc { get; set; }
		public IEnumerable< ShipmentItem > Items { get; set; }
		public string Carrier { get; set; }
		public string TrackingNumber { get; set; }
		public string Note { get; set; }

		public Shipment()
		{
		}

		internal Shipment( ShipmentResponse response )
		{
			this.Id = response.Id;
			this.OrderId = response.OrderId;
			this.OrderIncrementId = response.OrderIncrementId;
			this.CreatedAtUtc = response.CreatedAt;
			this.Items = response.Items?.Select( i => new ShipmentItem( i ) )?.ToList() ?? new List< ShipmentItem >();

			var trackingInfo = response.Tracks;
			if ( trackingInfo != null && trackingInfo.Any() )
			{
				this.Carrier = trackingInfo.First().Title;
				this.TrackingNumber = trackingInfo.First().TrackingNumber;
			}

			if ( response.Comments != null && response.Comments.Any() )
			{
				this.Note = response.Comments.OrderByDescending( c => c.CreatedAt ).First().Comment;
			}
		}
	}

	public class ShipmentItem
	{
		public string Sku { get; set; }
		public int Quantity { get; set; }
		public decimal? Weight { get; set; }

		public ShipmentItem()
		{
		}

		internal ShipmentItem( ShipmentResponseItem item )
		{
			this.Sku = item.Sku;
			this.Quantity = item.Quantity ?? 0;
			this.Weight = item.Weight;
		}
	}
}