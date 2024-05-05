import torch
import torchvision
import pprint


print(torch.__version__)
print(f"cuda, {torch.cuda.is_available()}")
print(torch.version.cuda)

print(f"compute_{''.join(map(str,(torch.cuda.get_device_capability())))}")
device_num:int = torch.cuda.device_count()
print(f"find gpu devices, {device_num}")
for idx in range(device_num):
    print(f"cuda:{idx}, {torch.cuda.get_device_name(idx)}")

print("torchvision.models")
pprint.pprint([s for s in dir(torchvision.models) if s[0].isupper()], compact=True)

print("torchvision.models.segmentation")
pprint.pprint([s for s in dir(torchvision.models.segmentation) if s[0].isupper()], compact=True)

print("torchvision.models.detection")
pprint.pprint([s for s in dir(torchvision.models.detection) if s[0].isupper()], compact=True)


print("end")