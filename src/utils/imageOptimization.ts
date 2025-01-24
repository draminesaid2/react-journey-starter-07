// Quality levels for different image sizes - reduced for faster loading
const QUALITY_LEVELS = {
  thumbnail: 30,  // Very low quality for thumbnails (reduced from 40)
  preview: 50,    // Medium quality for previews (reduced from 60)
  full: 70       // Higher quality for full view (reduced from 75)
};

export const optimizeImageUrl = (url: string, width: number = 800, quality?: 'thumbnail' | 'preview' | 'full'): string => {
  if (!url || url.includes('?w=') || !url.startsWith('http')) {
    return url;
  }

  let imageQuality = QUALITY_LEVELS.preview;
  if (quality) {
    imageQuality = QUALITY_LEVELS[quality];
  } else if (width <= 200) {
    imageQuality = QUALITY_LEVELS.thumbnail;
  } else if (width >= 800) {
    imageQuality = QUALITY_LEVELS.full;
  }

  const separator = url.includes('?') ? '&' : '?';
  return `${url}${separator}w=${width}&q=${imageQuality}&blur=1`;
};

export const preloadVideo = (src: string): Promise<void> => {
  return new Promise((resolve, reject) => {
    const video = document.createElement('video');
    
    video.preload = 'auto';
    video.muted = true;
    video.playsInline = true;
    
    video.onloadeddata = () => {
      resolve();
    };
    video.onerror = reject;
    
    video.src = src;
  });
};

export const preloadImage = (src: string, width: number = 800, quality?: 'thumbnail' | 'preview' | 'full'): Promise<void> => {
  const optimizedSrc = optimizeImageUrl(src, width, quality);
  
  return new Promise((resolve, reject) => {
    const img = new Image();
    
    img.onload = () => {
      resolve();
    };
    img.onerror = reject;
    
    img.loading = 'lazy';
    img.decoding = 'async';
    
    img.src = optimizedSrc;
  });
};

export const generateSrcSet = (src: string, quality?: 'thumbnail' | 'preview' | 'full'): string => {
  if (!src) return '';
  
  const sizes = [160, 320, 480, 640, 800];
  return sizes
    .map(size => `${optimizeImageUrl(src, size, quality)} ${size}w`)
    .join(', ');
};