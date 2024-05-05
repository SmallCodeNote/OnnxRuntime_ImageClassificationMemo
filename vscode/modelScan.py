import os
import shutil
import time

import torch
import torch.nn as nn
import torch.utils
from torch.utils.data import DataLoader, Dataset

from torchvision import datasets, models, transforms
import torch.onnx
import typing

def calculate_accuracy(model_path: str, dataloaders: dict[str, DataLoader], device: torch.device) -> float:
    # モデルをロード
    model = torch.load(model_path)
    model = model.to(device)
    model.eval()

    correct = 0
    total = 0

    with torch.no_grad():
        for data in dataloaders['train']:
            images, labels = data
            images = images.to(device)
            labels = labels.to(device)
            outputs = model(images)
            _, predicted = torch.max(outputs.data, 1)
            total += labels.size(0)
            correct += (predicted == labels).sum().item()

    accuracy = correct / total
    return accuracy

def trainRun(model:nn.Module, dataloaders:dict[str,any],device:torch.device ,saveDirectoryPath:str,reportFilePath:str):
    resultOnnxPath =os.path.join(saveDirectoryPath,model._get_name(),"best_model.onnx")
    resultPthPath = os.path.join(saveDirectoryPath,model._get_name(),"best_model.pth")
    best_model_epoch = 0

    model = model.to(device)

    if not os.path.exists(os.path.join(saveDirectoryPath,model._get_name())):
        os.makedirs(os.path.join(saveDirectoryPath,model._get_name()))    

    #optimizer = torch.optim.SGD(model.parameters(), lr=0.001,momentum=0.9)
    optimizer = torch.optim.Adam(model.parameters())

    criterion = torch.nn.CrossEntropyLoss()

    best_loss = float('inf')
    loss_history = ""

    batches = [batch for batch in dataloaders['train']]

    for epoch in range(num_epoch_max):
        running_loss = 0.0

        for inputs, labels in batches:
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

        loss_history = loss_history +f"\t{epoch_loss}"

        # modelSave
        if epoch_loss < best_loss:
            best_loss = epoch_loss
            torch.onnx.export(model, torch.randn(1, 3, 224, 224).to(device), resultOnnxPath, input_names=["inputTensor"], output_names=["outputArray"])
            torch.save(model, resultPthPath)
            best_model_epoch = epoch

    best_model_accuracy = calculate_accuracy(resultPthPath,dataloaders,device)
    f = open(reportFilePath, 'a')
    f.write(f"{model._get_name()}\t{best_model_accuracy}\t{best_loss}\t{best_model_epoch}\t{loss_history}\r\n")
    f.close()

if __name__ == '__main__':

    storeDirectoryPath = R"R:\store"
    reportFilePath = R"R:\report.txt"

    trainImageDirPath = R"R:\train"
    num_epoch_max = 50 

    saveDirectoryPath = R"R:\model"

    if not os.path.exists(saveDirectoryPath):
        os.makedirs(saveDirectoryPath)    

    data_transforms = {
        'train': transforms.Compose([
            transforms.ToTensor()
        ]),
    }

    # LoadImage
    
    image_datasets = {x: datasets.ImageFolder(trainImageDirPath, data_transforms[x]) for x in ['train']}
    dataloaders = {x: torch.utils.data.DataLoader(image_datasets[x], batch_size=64, shuffle=True, num_workers=os.cpu_count(),pin_memory=True) for x in ['train']}
    device = torch.device("cuda:0" if torch.cuda.is_available() else "cpu")
    torch.backends.cudnn.benchmark = True

    modelList = []
    modelList.append(models.alexnet(weights="DEFAULT"))
    modelList.append(models.squeezenet1_0(weights="DEFAULT"))
    modelList.append(models.densenet121(weights="DEFAULT"))
    modelList.append(models.mobilenet_v2(weights="DEFAULT"))
    modelList.append(models.mobilenet_v3_small(weights="DEFAULT"))
    modelList.append(models.resnet18(weights="DEFAULT"))
    modelList.append(models.vgg11(weights="DEFAULT"))

    class_count = 3

    for model in modelList:
        model_name = model._get_name()
        
        if isinstance(model, models.AlexNet) or isinstance(model, models.VGG):
            model.classifier[-1] = torch.nn.Linear(model.classifier[-1].in_features, class_count)
        elif isinstance(model, models.SqueezeNet):
            model.classifier[1] = torch.nn.Conv2d(512, class_count, kernel_size=(1,1), stride=(1,1))
        elif isinstance(model, models.DenseNet):
            model.classifier = torch.nn.Linear(model.classifier.in_features, class_count)
        elif isinstance(model, models.MobileNetV2) or isinstance(model, models.MobileNetV3):
            model.classifier[-1] = torch.nn.Linear(model.classifier[-1].in_features, class_count)
        elif isinstance(model, models.ResNet):
            model.fc = torch.nn.Linear(model.fc.in_features, class_count)


    for model in modelList:
        trainRun(model, dataloaders,device,saveDirectoryPath,reportFilePath)

        for filename in os.listdir(saveDirectoryPath):
            full_file_name = os.path.join(saveDirectoryPath, filename)
            shutil.move(full_file_name, storeDirectoryPath)

