using Entregable_Universities.Models;

namespace Entregable_Universities.Services
{
    public interface IstudentsService
    {
        IEnumerable<StudentModel> GetAllListStudents();
    }
}
