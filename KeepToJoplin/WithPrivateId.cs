namespace KeepToJoplin
{
	abstract class WithPrivateId
	{
		string _id = "";

		public void SetId(string id)
		{
			_id = id;
		}

		public string GetId()
		{
			return _id;
		}
	}
}