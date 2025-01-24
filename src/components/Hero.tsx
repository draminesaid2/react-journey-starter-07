import { ArrowRight } from "lucide-react";

const Hero = () => {
  return (
    <div className="min-h-screen flex items-center justify-center hero-gradient">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-20 text-center">
        <h1 className="text-4xl md:text-6xl font-bold text-white mb-6 animate-slide-up">
          Welcome to Your Next
          <br /> Amazing Project
        </h1>
        <p className="text-xl md:text-2xl text-white/80 mb-8 animate-slide-up" style={{ animationDelay: "0.1s" }}>
          Build something incredible with modern web technologies
        </p>
        <button className="bg-white text-blue-600 px-8 py-3 rounded-full font-medium inline-flex items-center gap-2 hover:bg-opacity-90 transition-all animate-slide-up shadow-lg" style={{ animationDelay: "0.2s" }}>
          Get Started
          <ArrowRight size={20} />
        </button>
      </div>
    </div>
  );
};

export default Hero;