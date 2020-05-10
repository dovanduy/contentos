using ContentProccessService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Models
{
    public class TasksViewModelReturn
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? PublishTime { get; set; }
        public int? IsActive { get; set; }
        public DateTime? StartedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public UsersModels Writer { get; set; }
        public UsersModels Editor { get; set; }
        public ContentModels Content { get; set; }
        public StatusModels Status { get; set; }
        public List<int> Tags { get; set; }
        public List<TagsViewModel> TagFull { get; set; }
        public Comments Comment { get; set; }
        public string Campaign { get; set; }
    }
    public class TasksViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? PublishTime { get; set; }
        public int? IsActive { get; set; }
        public DateTime? StartedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public UsersModels Writer { get; set; }
        public UsersModels Editor { get; set; }
        public ContentModels Content { get; set; }
        public StatusModels Status { get; set; }
        public List<TagsViewModel> Tags { get; set; }
        public List<int> Tag { get; set; }
        public Comments Comment { get; set; }
        public string Campaign { get; set; }
        public string EmailWriter { get; set; }
        public Dictionary<string, List<int>> listFanpages { get; set; }
        public UsersModels Customer { get; set; }
    }
    public class TasksViewModelMessage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Campaign { get; set; }
        public string EmailWriter { get; set; }
    }
    public class ContentModels
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
    }
    public class CampaignModels
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class TasksViewByEditorModel
    {
        public int Id { get; set; }
        public CampaignModels Campaign { get; set; }
        public string Title { get; set; }
        public StatusTaskModels Status { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? Deadline { get; set; }

    }
    public class TagsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class StatusTaskModels
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

    }

    public class UsersModels
    {
        public int? Id { get; set; }
        public string Name { get; set; }

    }
    public class StatusModels
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

    }
    public class StatusModelsReturn
    {
        public int? Id { get; set; }
        public string Name { get; set; }

    }

    public class TaskModel
    {

        public int Id { get; set; }
        public int IdCampaign { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? PublishTime { get; set; }
        public int? Status { get; set; }
        public DateTime? StartedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? IdWriter { get; set; }

    }
    public class Tag
    {
        public int Id { get; set; }
    }
    public class CreateTaskModel
    {
        public int IdCampaign { get; set; }
        public int IdWriter { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? PublishTime { get; set; }
        public List<int> Tags { get; set; }
    }
    public class UpdateTaskModel
    {
        public int Id { get; set; }
        public UsersModels Writer { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? PublishTime { get; set; }
        public List<TasksTags> Tags { get; set; }
    }
    public class ReturnUpdateTaskModel
    {
        public int Id { get; set; }
        public UsersModels Writer { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? PublishTime { get; set; }
        public List<TagsViewModel> Tags { get; set; }
        public StatusModels Status { get; set; }
    }
    //public class TaskDetailModel
    //{

    //    public UsersModels IdWriter { get; set; }
    //    public string Title { get; set; }
    //    public string Description { get; set; }
    //    public DateTime? Deadline { get; set; }
    //    public DateTime? PublishTime { get; set; }
    //    public List<TasksTags> Tags { get; set; }
    //}
}
