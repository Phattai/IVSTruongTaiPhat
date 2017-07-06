using System;

namespace Core.Interface
{
    public abstract class IDTO
    {
        public int? page { get; set; } = 1;

        public int? created_by { get; set; } = 1;

        public DateTime? created_datetime { get; set; }

        public int? updated_by { get; set; } = 2;

        public DateTime? updated_datetime { get; set; }

    }
}
