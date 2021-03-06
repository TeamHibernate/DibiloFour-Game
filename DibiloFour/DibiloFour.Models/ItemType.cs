﻿namespace DibiloFour.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ItemType
    {
        #region Fields
        #endregion

        #region Constructors
        public ItemType()
        {

        }

        public ItemType(string name)
        {
            this.Name = name;
        }

        public ItemType(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }
        #endregion
    }
}