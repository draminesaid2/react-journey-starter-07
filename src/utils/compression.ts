import { FFmpeg } from '@ffmpeg/ffmpeg';
import { fetchFile, toBlobURL } from '@ffmpeg/util';

interface CompressionProgress {
  percent: number;
  timeLeft: string;
  currentSize: number;
  targetSize: number;
  speed: string;
}

export const formatFileSize = (bytes: number): string => {
  if (bytes === 0) return '0 Bytes';
  const k = 1024;
  const sizes = ['Bytes', 'KB', 'MB', 'GB'];
  const i = Math.floor(Math.log(bytes) / Math.log(k));
  return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
};

const calculateTimeLeft = (progress: number, elapsedTime: number): string => {
  if (progress === 0) return 'Calculating...';
  const timeLeft = (elapsedTime / progress) * (100 - progress);
  const minutes = Math.floor(timeLeft / 60000);
  const seconds = Math.floor((timeLeft % 60000) / 1000);
  return `${minutes}m ${seconds}s`;
};

export const compressVideo = async (
  file: File,
  targetSize: number = 400 * 1024 * 1024, // 400MB target
  onProgress: (progress: CompressionProgress) => void
): Promise<File> => {
  const ff = new FFmpeg();
  const startTime = Date.now();
  let lastProgress = 0;
  let compressionSpeed = '0x';

  try {
    console.log('Loading FFmpeg...');
    const baseURL = 'https://unpkg.com/@ffmpeg/core@0.12.6/dist/esm';
    
    ff.on('log', ({ message }) => {
      if (message.includes('speed=')) {
        compressionSpeed = message.split('speed=')[1].split('x')[0] + 'x';
      }
      console.log('FFmpeg Log:', message);
    });

    ff.on('progress', ({ progress, time }) => {
      const currentTime = Date.now();
      const elapsedTime = currentTime - startTime;
      const progressPercent = Math.round(progress * 100);
      
      if (progressPercent !== lastProgress) {
        lastProgress = progressPercent;
        onProgress({
          percent: progressPercent,
          timeLeft: calculateTimeLeft(progressPercent, elapsedTime),
          currentSize: 0, // Will be updated after compression
          targetSize,
          speed: compressionSpeed
        });
      }
    });

    await ff.load({
      coreURL: await toBlobURL(`${baseURL}/ffmpeg-core.js`, 'text/javascript'),
      wasmURL: await toBlobURL(`${baseURL}/ffmpeg-core.wasm`, 'application/wasm'),
    });

    const inputFileName = 'input.mp4';
    const outputFileName = 'output.webm';

    console.log('Writing input file...');
    await ff.writeFile(inputFileName, await fetchFile(file));
    
    // Calculate bitrate based on target size
    const duration = await getVideoDuration(file);
    const targetBitrate = Math.floor((targetSize * 8) / duration); // bits per second
    const videoBitrate = Math.floor(targetBitrate * 0.95); // 95% for video
    const audioBitrate = Math.floor(targetBitrate * 0.05); // 5% for audio

    console.log('Starting FFmpeg compression...');
    await ff.exec([
      '-i', inputFileName,
      '-c:v', 'libvpx-vp9',
      '-cpu-used', '4',
      '-deadline', 'realtime',
      '-threads', '8',
      '-tile-columns', '4',
      '-frame-parallel', '1',
      '-b:v', `${videoBitrate}`,
      '-maxrate', `${videoBitrate * 1.5}`,
      '-bufsize', `${videoBitrate * 2}`,
      '-vf', 'scale=1280:-2', // 720p
      '-c:a', 'libopus',
      '-b:a', `${audioBitrate}`,
      '-row-mt', '1',
      outputFileName
    ]);

    console.log('Reading compressed file...');
    const data = await ff.readFile(outputFileName);
    const compressedFile = new File([data], file.name.replace('.mp4', '.webm'), { type: 'video/webm' });
    
    // Final progress update with actual compressed size
    onProgress({
      percent: 100,
      timeLeft: '0s',
      currentSize: compressedFile.size,
      targetSize,
      speed: compressionSpeed
    });

    console.log('Compression complete:', {
      originalSize: formatFileSize(file.size),
      compressedSize: formatFileSize(compressedFile.size),
      compressionRatio: `${((1 - compressedFile.size / file.size) * 100).toFixed(1)}%`
    });

    return compressedFile;
  } catch (error) {
    console.error('Compression error:', error);
    throw error;
  }
};

const getVideoDuration = async (file: File): Promise<number> => {
  return new Promise((resolve) => {
    const video = document.createElement('video');
    video.preload = 'metadata';
    video.onloadedmetadata = () => {
      window.URL.revokeObjectURL(video.src);
      resolve(video.duration);
    };
    video.src = URL.createObjectURL(file);
  });
};