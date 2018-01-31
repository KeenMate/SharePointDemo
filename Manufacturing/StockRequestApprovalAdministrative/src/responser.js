export default {
	PostApprove: function (item, data, user) {
		if (
			data.includes("There is nothing more to do. Everything was saved.")
		) {
			item.ApprovedBy.push(user);
			if (item.ApprovedBy.sort().equals(item.AllowedApprovers.sort()))
				item.Status = "Approved";
			else item.Status = "Waiting for approval";
		} else if (
			data == "This item was rejected. You can not approve it anymore."
		) {
			item.Status = "Rejected";
			return data;
		} else if (
			data ==
			"This item was already approved. You can not approve it anymore."
		) {
			item.Status = "Approved";
			item.ApprovedBy = ["Please refresh page to view approvers."];
			return data;
		} else if (
			data.includes(
				"Too late to approve this request. The request should be delivered on"
			)
		) {
			item.Status = "Waiting for approval";
			return data;
		} else if (data == "You are not allowed to approve this request.") {
			item.Status = "Waiting for approval";
			return data;
		} else if (data == "You already approved this item.") {
			item.ApprovedBy.push(self.currentUser);
			return "You already approved this item.";
		} else if (
			data ==
			"An unautorized person approved this request. Please contact your administrator."
		) {
			item.Status = "Waiting for approval";
			return data;
		} else if (data.includes("There is no item with response id")) {
			item.Status = "Waiting for approval";
			return data;
		} else {
			item.Status = "Waiting for approval";
			return data
		};
	},
	PostReject: function (item, data, user) {
		if (
			data.includes("There is nothing more to do. Everything was saved.")
		) {
			item.ModifiedBy = user;
			item.Status = "Rejected";
		} else if (data == "This item was already rejected.") {
			item.Status = "Rejected";
			item.ModifiedBy = "Please refresh page to view rejector.";
			return data;
		} else if (
			data == "This item was approved. You can not reject it anymore."
		) {
			item.Status = "Approved";
			item.ApprovedBy = ["Please refresh page to view approvers."];
			return data;
		} else if (data == "You are not allowed to reject this request.") {
			item.Status = "Waiting for approval";
			return data;
		} else if (data.includes("There is no item with response id")) {
			item.Status = "Waiting for approval";
			return data;
		} else {
			item.Status = "Waiting for approval";
			return data;
		}
	}
}