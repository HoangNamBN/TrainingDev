namespace eTrainingSolution.Data.Entities
{
    public class Role
    {
        /// <summary>
        /// mã quyền
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// tên quyền
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// mô tả
        /// </summary>
        public string Description { get; set; }
    }
}
