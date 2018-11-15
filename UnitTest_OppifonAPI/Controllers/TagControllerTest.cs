using System.Collections.Generic;
using System.Linq;
using DAL.Factory;
using DAL.Models;
using DAL.Models.ManyToMany;
using DAL.Repositories;
using DAL.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OppifonAPI.Controllers;

namespace UnitTest_OppifonAPI.Controllers
{
    [TestClass]
    public class TagControllerTest
    {
        private TagController _uut;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IFactory> _factoryMock;
        private Mock<ITagRepository> _tagRepositoryMock;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _factoryMock = new Mock<IFactory>();
            _factoryMock.Setup(x => x.GetUOF()).Returns(_unitOfWorkMock.Object);
            _uut = new TagController(_factoryMock.Object);
        }

        [TestMethod]
        public void GetAllTagsShouldReturnCorrectly()
        {
            var tag1 = new Tag {Name = "Tag1"};
            var tag2 = new Tag {Name = "Tag2"};
            var tag3 = new Tag {Name = "Tag3"};
            var tags = new List<Tag>
            {
                tag1, tag2, tag3
            };
            _unitOfWorkMock.Setup(x => x.Tags.GetAll())
                .Returns(tags.AsQueryable);

            var actResult = _uut.GetAllTags();

            Assert.AreEqual(actResult.Value[0], "Tag1");
            Assert.AreEqual(actResult.Value[1], "Tag2");
            Assert.AreEqual(actResult.Value[2], "Tag3");
        }

        [TestMethod]
        public void GetTagShouldReturnCorrectly()
        {
            var tagForExpert = new Tag {Name = "expertTag"};
            var expertTag = new ExpertTag {Tag = tagForExpert};
            var tagForMainField = new Tag {Name = "mainFieldTag"};
            var mainFieldTag = new MainFieldTag {Tag = tagForMainField};
            var review = new Review {Name = "review",};
            var expert = new Expert
            {
                ExpertCategory = new Category {Name = "category"},
                ExpertTags = new List<ExpertTag> {expertTag},
                MainFields = new List<MainFieldTag> {mainFieldTag},
                Reviews = new List<Review> {review}
            };

            var experts = new List<Expert> {expert};

            _unitOfWorkMock.Setup(x =>
                    x.Experts.GetExpertsWithTagName(It.IsAny<string>()))
                .Returns(experts.AsQueryable);

            var actResult = _uut.GetAllExpertsWithTag("dummy");
            var returnedExpert = actResult.Value[0];

            Assert.AreEqual(returnedExpert.ExpertCategory,
                expert.ExpertCategory.Name);
            Assert.AreEqual(returnedExpert.ExpertTags.ElementAt(0),
                expert.ExpertTags.ElementAt(0).Tag.Name);
            Assert.AreEqual(returnedExpert.MainFields.ElementAt(0),
                expert.MainFields.ElementAt(0).Tag.Name);
            Assert.AreEqual(returnedExpert.Reviews.ElementAt(0).Name,
                expert.Reviews.ElementAt(0).Name);
        }
    }
}