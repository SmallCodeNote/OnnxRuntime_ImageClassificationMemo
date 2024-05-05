import torch
from torchvision import transforms
from PIL import Image

# モデルのパス
model_path = R"R:\model\best_model.pth"

# 推論を行う画像のパス
image_paths = [R"R:\train\1\0000.jpg",R"R:\train\1\0010.jpg",R"R:\train\1\0020.jpg"
               ,R"R:\train\2\0000.jpg",R"R:\train\2\0010.jpg",R"R:\train\2\0020.jpg"
               ,R"R:\train\3\0000.jpg",R"R:\train\3\0010.jpg",R"R:\train\3\0020.jpg"]

# モデルの読み込み
model = torch.load(model_path)
model.eval()


for image_path in image_paths:
    # 画像の読み込みと前処理
    image = Image.open(image_path)
    preprocess = transforms.Compose([
        transforms.ToTensor()
    ])
    input_tensor = preprocess(image)
    input_batch = input_tensor.unsqueeze(0)

    # GPUが利用可能ならGPUを使用
    if torch.cuda.is_available():
        input_batch = input_batch.to('cuda')
        model.to('cuda')

    with torch.no_grad():
        output = model(input_batch)

    # 推論結果

    _, predicted = torch.max(output, 1)
    print('Predicted:', predicted.item())
