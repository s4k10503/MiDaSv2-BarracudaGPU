# MiDaSv2 BarracudaGPU

MiDaSv2 BarracudaGPU is an implementation of depth estimation with the [MiDaS](https://arxiv.org/abs/1907.01341v2) using Unity Barracuda.  
Preprocessing, inference, and postprocessing are all processed on the GPU using Unity Barracuda and Compute Shader.  

![movie_001](https://github.com/s4k10503/MiDaSv2-BarracudaGPU/assets/50241623/832b8bdc-8d2e-4a06-ab21-733c1e0549ad)  

## Requirements

Unity 2021.3.20f1 LTS or later

## Packages

- [Barracuda](https://github.com/Unity-Technologies/barracuda-release)
- [TestTools](https://github.com/keijiro/TestTools)

## ONNX file

[081_MiDaS_v2](https://github.com/PINTO0309/PINTO_model_zoo/tree/main/081_MiDaS_v2)

The ONNX model contained in this repo was converted by [PINTO0309](https://github.com/PINTO0309/PINTO_model_zoo).  
The original model was trained by [Intelligent Systems Lab Org](https://github.com/isl-org/MiDaS).  

## References

[MiDaSV2BarracudaCpu](https://github.com/SatoshiRobatoFujimoto/MiDaSV2BarracudaCpu)
