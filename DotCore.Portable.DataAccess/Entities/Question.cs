using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace DotCore.Portable.DataAccess.Entities
{
    public class Question
    {
        #region Properties, Indexers

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected internal set; }

        [Required]
        public string Title { get; protected internal set; }

        [Required]
        public string Description { get; protected internal set; }

        public DateTime CreatedOn { get; protected internal set; }

        public int? AcceptedAnswerId { get; protected internal set; }

        public Answer AcceptedAnswer { get; protected internal set; }


        public string UserId { get; protected internal set; }

        [Required]
        public User User { get; protected internal set; }

        public IList<Answer> Answers { get; protected internal set; }

        #endregion

        #region Constructors

        protected internal Question()
        {
        }

        /// <exception cref="ArgumentNullException"><paramref name="title"/> or <paramref name="description"/> or <paramref name="userId"/> is <see langword="null"/></exception>
        public Question([NotNull] string title, [NotNull] string description, [NotNull] string userId)
        {
            if (title == null)
                throw new ArgumentNullException(nameof(title));
            if (description == null)
                throw new ArgumentNullException(nameof(description));
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));

            Title = title;
            Description = description;
            UserId = userId;

            CreatedOn = DateTime.UtcNow;
        }

        #endregion

        #region Methods

        /// <exception cref="ArgumentNullException"><paramref name="answer"/> is <see langword="null"/></exception>
        public void AcceptAnswer(Answer answer)
        {
            if (answer == null)
                throw new ArgumentNullException(nameof(answer));

            AcceptedAnswerId = answer.Id;
            AcceptedAnswer = answer;
        }

        #endregion
    }
}