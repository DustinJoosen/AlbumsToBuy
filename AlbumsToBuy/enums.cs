using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy
{
	public enum AlbumType
	{
		CD,
		LP
	}
	
	public enum OrderStatus
	{
		Recieved,
		Processed,
		Send,
		Delivered
	}

	public enum PaymentStatus
	{
		NotPayed,
		Processing,
		Payed
	}

	public enum UserRole
	{
		UnconfirmedCustomer,
		Customer,
		Admin
	}

	public enum ShopSearchType
	{
		Name,
		Creator
	}
}
