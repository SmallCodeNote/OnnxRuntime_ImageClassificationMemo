import os
import torch

from torchvision import datasets, models, transforms
import torch.onnx
import onnx
import onnxruntime

if __name__ == '__main__':

    trainImageDirPath = R"R:\train"
    num_epoch_max = 25 
    resultOnnxPath = R"R:\model\best_model.onnx" 
    resultPthPath = R"R:\model\best_model.pth" 

    directoryPath = os.path.dirname(resultOnnxPath)
    if not os.path.exists(directoryPath):
        os.makedirs(directoryPath)    

    directoryPath = os.path.dirname(resultPthPath)
    if not os.path.exists(directoryPath):
        os.makedirs(directoryPath)    

    data_transforms = {
        'train': transforms.Compose([
            transforms.ToTensor()
        ]),
    }

    # LoadImage
    image_datasets = {x: datasets.ImageFolder(trainImageDirPath, data_transforms[x]) for x in ['train']}
    dataloaders = {x: torch.utils.data.DataLoader(image_datasets[x], batch_size=128, shuffle=True, num_workers=os.cpu_count()) for x in ['train']}
    device = torch.device("cuda:0" if torch.cuda.is_available() else "cpu")
    torch.backends.cudnn.benchmark = True

    if os.path.exists(resultPthPath):
        model = torch.load(resultPthPath).to(device)
    else:
        #model = models.mobilenet_v2(weights="DEFAULT")
        #model = models.mobilenet_v2(weights=None)
        #model.classifier[-1]= torch.nn.Linear(model.classifier[-1].in_features, 3)

        model = models.alexnet(weights="DEFAULT")
        #model = models.alexnet(weights=None)
        #model.fc = torch.nn.Linear(model.fc.in_features, 3)
        model.classifier[-1]= torch.nn.Linear(model.classifier[-1].in_features, 3)

        
        model = model.to(device)

    # Train
    criterion = torch.nn.CrossEntropyLoss()
    #optimizer = torch.optim.SGD(model.parameters(), lr=0.001,momentum=0.9)
    optimizer = torch.optim.Adam(model.parameters())
    best_loss = float('inf')
    for epoch in range(num_epoch_max):
        running_loss = 0.0
        for inputs, labels in dataloaders['train']:
            inputs = inputs.to(device)
            labels = labels.to(device)

            optimizer.zero_grad()

            outputs = model(inputs)
            loss = criterion(outputs, labels)

            loss.backward()
            optimizer.step()

            running_loss += loss.item() * inputs.size(0)

        epoch_loss = running_loss / len(dataloaders['train'].dataset)
        print(f'Epoch {epoch+1}/{num_epoch_max}, Loss: {epoch_loss}')

        # modelSave
        if epoch_loss < best_loss:
            best_loss = epoch_loss
            torch.onnx.export(model, torch.randn(1, 3, 224, 224).to(device), resultOnnxPath, input_names=["inputTensor"], output_names=["outputArray"])
            torch.save(model, resultPthPath)

