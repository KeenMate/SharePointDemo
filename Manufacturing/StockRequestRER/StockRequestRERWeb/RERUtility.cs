using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint.Client;

namespace StockRequestRERWeb
{
	public class RERUtility
	{
		public static void AddListItemRemoteEventReceiver(ClientContext context, string listName, EventReceiverType eventType,
			EventReceiverSynchronization synchronization, string receiverName,
			string receiverUrl, int sequence, string receiverAssemblyName = "", string receiverClassName = "")
		{
			var list = context.Web.Lists.GetByTitle(listName);
			context.Load(list);
			var eventReceivers = list.EventReceivers;
			context.Load(eventReceivers);
			context.ExecuteQuery();
			var newRER = new EventReceiverDefinitionCreationInformation();
			newRER.EventType = eventType;
			newRER.ReceiverName = receiverName;
			newRER.ReceiverClass = receiverClassName;
			newRER.ReceiverAssembly = receiverAssemblyName;
			newRER.ReceiverUrl = receiverUrl;
			newRER.Synchronization = synchronization;
			newRER.SequenceNumber = sequence;
			list.EventReceivers.Add(newRER);
			list.Update();
			context.ExecuteQuery();
		}
	}
}