using System.Text.Json;
using CompanyManagementSystem.Web.Models;

namespace CompanyManagementSystem.Web.Services
{
    public class VersionControlService
    {
        private static List<Version> _versions = new List<Version>();
        private static int _currentVersionId = 0;

        public Version CreateVersion(string entityType, int entityId, object currentState, string changes, string changedBy)
        {
            var lastVersion = _versions
                .Where(v => v.EntityType == entityType && v.EntityId == entityId)
                .OrderByDescending(v => v.CreatedAt)
                .FirstOrDefault();

            var versionNumber = lastVersion == null ? "1.0" : IncrementVersion(lastVersion.VersionNumber);
            
            var version = new Version
            {
                Id = ++_currentVersionId,
                EntityType = entityType,
                EntityId = entityId,
                VersionNumber = versionNumber,
                Changes = changes,
                ChangedBy = changedBy,
                CreatedAt = DateTime.Now,
                PreviousVersion = lastVersion?.VersionNumber ?? "",
                CurrentState = JsonSerializer.Serialize(currentState)
            };

            _versions.Add(version);
            return version;
        }

        public List<Version> GetVersions(string entityType, int entityId)
        {
            return _versions
                .Where(v => v.EntityType == entityType && v.EntityId == entityId)
                .OrderByDescending(v => v.CreatedAt)
                .ToList();
        }

        public Version? GetVersion(string entityType, int entityId, string versionNumber)
        {
            return _versions
                .FirstOrDefault(v => v.EntityType == entityType && 
                                   v.EntityId == entityId && 
                                   v.VersionNumber == versionNumber);
        }

        private string IncrementVersion(string currentVersion)
        {
            var parts = currentVersion.Split('.');
            if (parts.Length != 2) return "1.0";

            if (int.TryParse(parts[0], out int major) && int.TryParse(parts[1], out int minor))
            {
                return $"{major}.{minor + 1}";
            }

            return "1.0";
        }
    }
} 