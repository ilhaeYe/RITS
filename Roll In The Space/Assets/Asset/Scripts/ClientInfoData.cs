public static class ClientInfoData {
	public static int rank = -1;
	public static string fb_id = "";
	public static string fb_first_name = "";
	public static string fb_last_name = "";
	public static string fb_full_name = "Guest";
	public static int score = 0;

	public static void SetClinetName(string _fb_id = "", string _fb_first_name = "", string _fb_last_name = "")
	{
		fb_id = _fb_id;
		fb_first_name = _fb_first_name;
		fb_last_name = _fb_last_name;
		fb_full_name = _fb_first_name + "_" + fb_last_name;
	}
	public static void ClearClientData()
	{
		rank = -1;
		fb_id = "";
		fb_first_name = "";
		fb_last_name = "";
		fb_full_name = "Guest";
		score = 0;
	}

	public static void SetScore(int _score)
	{
		score = _score;
	}
	public static void SetRank(int _rank)
	{
		rank = _rank;
	}
}
