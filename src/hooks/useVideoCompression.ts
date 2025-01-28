import { useState, useRef, useEffect } from 'react';
import { FFmpeg } from '@ffmpeg/ffmpeg';
import { fetchFile, toBlobURL } from '@ffmpeg/util';
import { useToast } from '@/hooks/use-toast';

export const useVideoCompression = () => {
  const [loaded, setLoaded] = useState(false);
  const [isCompressing, setIsCompressing] = useState(false);
  const [compressionProgress, setCompressionProgress] = useState(0);
  const [originalSize, setOriginalSize] = useState<number | null>(null);
  const [compressedSize, setCompressedSize] = useState<number | null>(null);
  const [loadingMessage, setLoadingMessage] = useState('Loading FFmpeg...');
  const [timeLeft, setTimeLeft] = useState('Calcul...');
  const [speed, setSpeed] = useState('0x');
  const startTimeRef = useRef<number>(0);
  const ffmpegRef = useRef(new FFmpeg());
  const { toast } = useToast();

  useEffect(() => {
    loadFFmpeg();
    return () => {
      const ffmpeg = ffmpegRef.current;
      if (ffmpeg) {
        ffmpeg.terminate();
      }
    };
  }, []);

  const loadFFmpeg = async () => {
    const ffmpeg = ffmpegRef.current;
    try {
      console.log('Loading FFmpeg...');
      setLoadingMessage('Downloading FFmpeg...');
      const baseURL = 'https://unpkg.com/@ffmpeg/core@0.12.6/dist/esm';
      
      ffmpeg.on('log', ({ message }) => {
        console.log('FFmpeg Log:', message);
        if (message.includes('speed=')) {
          const speedMatch = message.match(/speed=\s*(\d+\.\d+)x/);
          if (speedMatch) {
            setSpeed(speedMatch[1] + 'x');
          }
        }
      });

      ffmpeg.on('progress', ({ progress, time }) => {
        const percentage = Math.round(progress * 100);
        setCompressionProgress(percentage);
        
        // Calculate time left
        const elapsedTime = Date.now() - startTimeRef.current;
        const estimatedTotalTime = elapsedTime / progress;
        const remainingTime = estimatedTotalTime - elapsedTime;
        
        const minutes = Math.floor(remainingTime / 60000);
        const seconds = Math.floor((remainingTime % 60000) / 1000);
        setTimeLeft(`${minutes}m ${seconds}s`);
        
        setLoadingMessage(`Processing: ${percentage}% (${(time / 1000000).toFixed(1)}s)`);
      });

      await ffmpeg.load({
        coreURL: await toBlobURL(`${baseURL}/ffmpeg-core.js`, 'text/javascript'),
        wasmURL: await toBlobURL(`${baseURL}/ffmpeg-core.wasm`, 'application/wasm'),
      });
      
      setLoaded(true);
      setLoadingMessage('');
    } catch (error) {
      console.error('Error loading FFmpeg:', error);
      toast({
        variant: "destructive",
        title: "Error",
        description: "Failed to load video compression. Please refresh and try again."
      });
    }
  };

  const handleFileCompression = async (file: File, targetSize: number) => {
    if (!loaded) {
      toast({
        title: "Please wait",
        description: "Video compression is still loading..."
      });
      return null;
    }

    const ffmpeg = ffmpegRef.current;
    setIsCompressing(true);
    setOriginalSize(file.size);
    setCompressionProgress(0);
    setTimeLeft('Calcul...');
    setSpeed('0x');
    startTimeRef.current = Date.now();

    try {
      console.log('Starting video compression...');
      await ffmpeg.writeFile('input.mp4', await fetchFile(file));
      
      const duration = await getVideoDuration(file);
      const targetBitrate = Math.floor((targetSize * 8) / duration);
      const videoBitrate = Math.floor(targetBitrate * 0.95);
      const audioBitrate = Math.floor(targetBitrate * 0.05);

      // Updated compression command with better quality settings
      await ffmpeg.exec([
        '-i', 'input.mp4',
        '-c:v', 'libx264',
        '-preset', 'medium',
        '-crf', '23',
        '-b:v', `${videoBitrate}`,
        '-maxrate', `${videoBitrate * 2}`,
        '-bufsize', `${videoBitrate * 3}`,
        '-c:a', 'aac',
        '-b:a', `${audioBitrate}`,
        '-movflags', '+faststart',
        '-y',
        'output.mp4'
      ]);

      const data = await ffmpeg.readFile('output.mp4');
      const compressedBlob = new Blob([data], { type: 'video/mp4' });
      setCompressedSize(compressedBlob.size);
      
      const compressedFile = new File([compressedBlob], file.name, {
        type: 'video/mp4',
      });

      console.log('Compression complete', {
        originalSize: file.size,
        compressedSize: compressedFile.size,
        reduction: ((file.size - compressedFile.size) / file.size * 100).toFixed(1) + '%'
      });

      setLoadingMessage('Compression complete!');
      setCompressionProgress(100);
      
      return compressedFile;
    } catch (error) {
      console.error('Error during compression:', error);
      toast({
        variant: "destructive",
        title: "Compression Error",
        description: "Failed to compress video. Please try again."
      });
      return null;
    } finally {
      setIsCompressing(false);
    }
  };

  const cancelCompression = () => {
    const ffmpeg = ffmpegRef.current;
    if (ffmpeg && isCompressing) {
      ffmpeg.terminate();
      loadFFmpeg(); // Reload FFmpeg after termination
      setIsCompressing(false);
      setCompressionProgress(0);
      setTimeLeft('Cancelled');
      setSpeed('0x');
      toast({
        title: "Compression cancelled",
        description: "Video compression was cancelled by user"
      });
    }
  };

  return {
    isCompressing,
    originalSize,
    compressedSize,
    compressionProgress,
    loadingMessage,
    timeLeft,
    speed,
    handleFileCompression,
    cancelCompression
  };
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