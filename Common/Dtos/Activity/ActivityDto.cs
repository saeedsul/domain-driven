namespace Common.Dtos.Activity
{
    public class ActivityDto
    {
        public Guid Id { get; set; }
         
        public string? Name { get; set; }
         
        public string? FromAddress { get; set; }
         
        public string? ToEmailAddress { get; set; }
         
        public string? FromName { get; set; }
         
        public DateTime CreatedDate { get; set; }
         
        public DateTime? BouncedDate { get; set; }
         
        public DateTime? OpenedDate { get; set; }
         
        public DateTime? SentDate { get; set; }
    }
}
