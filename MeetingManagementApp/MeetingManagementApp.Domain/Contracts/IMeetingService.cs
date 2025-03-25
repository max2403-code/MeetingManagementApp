using MeetingManagementApp.Domain.Models.DTO;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface IMeetingService
    {
        public IReadOnlyCollection<MeetingDTO> GetMeetingsOnDate(DateTime date);
        
        int AddNewMeeting(MeetingDTO meeting);

        string? ValidateMeeting(MeetingDTO meeting);
        
        int RemoveMeeting(int id);

        int UpdateMeeting(MeetingDTO meeting);

        MeetingDTO GetMeetingById(int id);

        Task SaveMeetingsOnDateFileAsync(DateTime onDate, string folderPath);
    }
}
