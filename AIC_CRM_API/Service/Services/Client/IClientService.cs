using Service.Services.Client.DTOs;
using Utility.OutputData;

namespace Service.Services.Client
{
    public interface IClientService
    {
        Task<OutputDTO<ClientInput>> ClientAddUpdate(ClientInput input);
        Task<OutputDTO<bool>> ClientDelete(int id);
        Task<OutputDTO<List<ClientInput>>> ClientsListGet();
        Task<OutputDTO<ClientInput>> SingleClientGet(int id);

        #region Client Manager
        Task<OutputDTO<ClientManagerDto>> ClientManagerAddUpdate(ClientManagerDto input);
        Task<OutputDTO<bool>> ClientManagerDelete(int id);
        Task<OutputDTO<List<ClientManagerDto>>> ClientManagersListGet();
        Task<OutputDTO<ClientManagerDto>> ClientManagerGet(int id);
        Task<OutputDTO<List<ClientManagerDto>>> ClientManagerGetByClientId(int id);
        Task<OutputDTO<List<ClientManagerDto>>> GetWorkUnderManagers(int id);
        #endregion

        #region Client Pipeline
        Task<OutputDTO<bool>> ClientPipelineAddUpdate(ClientPipelineDto input);
        Task<OutputDTO<bool>> ClientPipelineDelete(int id);
        Task<OutputDTO<List<ClientPipelineDto>>> ClientPipelinesListGet();
        Task<OutputDTO<ClientPipelineDto>> ClientPipelineGet(int id);
        #endregion
    }
}
