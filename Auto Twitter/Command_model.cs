namespace Auto_Twitter
{
    internal class Command_model
    {
        public string name { get; set; }
        public string content { get; set; }
        public string image { get; set; }
        public string data { get; set; }

        public string command_name
        {
            get
            {
                return $"{name}";
            }
        }
    }
}
