using kriez.HomeDashboard.Data;
using kriez.HomeDashboard.Data.Models;
using Microsoft.EntityFrameworkCore;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Groups;
using Q42.HueApi.ColorConverters.Original;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeDashboard.HueWorkerService.Work
{
    public class Worker
    {
        private readonly DatabaseContext _context;
        private readonly ILocalHueClient _client;

        private List<HueLight> Lights;
        private List<HueScene> Groups;

        public Worker(DatabaseContext context, string ipAdress, string appKey)
        {
            _context = context;
            _client = new LocalHueClient(ipAdress);
            _client.Initialize(appKey);

            Lights = new List<HueLight>();
            Groups = new List<HueScene>();
        }

        public async Task ScanLightsAsync()
        {
            foreach (var item in (await _client.GetLightsAsync()))
            {
                HueLight hueLight = new HueLight()
                {
                    Id = item.Id,
                    IsOn = item.State.On,
                    IsReachable = item.State.IsReachable,
                    Name = item.Name,
                    Brightness = item.State.Brightness,
                    Color = item.State.ToHex()
                };

                Lights.Add(hueLight);
            }
        }

        public async Task ScanGroupsAsync()
        {
            var groups = (await _client.GetGroupsAsync()).Where(g => g.Type.Equals(GroupType.Room));
            foreach (var item in groups)
            {
                HueScene group = new HueScene()
                {
                    Id = item.Id,
                    Name = item.Name
                };

                foreach (var lightItem in item.Lights)
                {
                    var light = Lights.Single(l => l.Id.Equals(lightItem));
                    light.Group = group.Id;
                }

                Groups.Add(group);
            }
        }

        public async Task SaveAsync()
        {
            await SaveLightsAsync();
            await SaveGroupsAsync();
        }

        private async Task SaveLightsAsync()
        {
            foreach (var item in Lights)
            {
                var light = await _context.HueLights.SingleOrDefaultAsync(l => l.Id.Equals(item.Id));
                if (light == null)
                {
                    _context.HueLights.Add(item);
                }
                else if (!light.Equals(item))
                {
                    _context.Entry(item).State = EntityState.Modified;
                }
            }

            foreach (var item in _context.HueLights.Where(p => !Lights.Select(l => l.Id).ToList().Contains(p.Id)).ToList())
            {
                _context.HueLights.Remove(item);
            }
        }

        private async Task SaveGroupsAsync()
        {
            foreach (var item in Groups)
            {
                var group = await _context.HueScenes.SingleOrDefaultAsync(g => g.Id.Equals(item.Id));
                if (group == null)
                {
                    _context.HueScenes.Add(item);
                }
                else if (!group.Equals(item))
                {
                    _context.Entry(item).State = EntityState.Modified;
                }
            }

            foreach (var group in _context.HueScenes.Where(p => !Groups.Select(g => g.Id).ToList().Contains(p.Id)).ToList())
            {
                _context.HueScenes.Remove(group);
            }
        }
    }
}
