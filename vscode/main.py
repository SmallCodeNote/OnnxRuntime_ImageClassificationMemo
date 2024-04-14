import os
import torch
from torchvision import datasets, models, transforms
import torch.onnx
import onnx
import onnxruntime

if __name__ == '__main__':
    trainImageDirPath = R"R:\train"  # ここにディレクトリのパスを指定してください
    num_epoch_max = 10  # 実行予定エポック数を指定してください
    resultOnnxPath = R"R:\model\best_model.onnx"  # ONNXファイルのパスを指定してください
    resultPthPath = R"R:\model\best_model.pth"  # ONNXファイルのパスを指定してください

    data_transforms = {
        'train': transforms.Compose([
            transforms.RandomResizedCrop(224),
            transforms.RandomHorizontalFlip(),
            transforms.ToTensor(),
            transforms.Normalize([0.485, 0.456, 0.406], [0.229, 0.224, 0.225])
        ]),
    }

    # LoadImage
    image_datasets = {x: datasets.ImageFolder(trainImageDirPath, data_transforms[x]) for x in ['train']}
    dataloaders = {x: torch.utils.data.DataLoader(image_datasets[x], batch_size=4, shuffle=True, num_workers=4) for x in ['train']}
    device = torch.device("cuda:0" if torch.cuda.is_available() else "cpu")

    if os.path.exists(resultPthPath):
        model = torch.load(resultPthPath).to(device)
    else:
        model = models.mobilenet_v2(pretrained=True).to(device)

    # Train
    criterion = torch.nn.CrossEntropyLoss()
    optimizer = torch.optim.SGD(model.parameters(), lr=0.001)
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
